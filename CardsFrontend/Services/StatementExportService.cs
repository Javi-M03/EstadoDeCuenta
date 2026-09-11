using CardsFrontend.Models;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CardsFrontend.Services;

public interface IStatementExportService
{
    byte[] BuildPdf(Statement statement);
    byte[] BuildExcel(Statement statement);
}

/// <summary>
/// Genera el estado de cuenta de una tarjeta como PDF (QuestPDF)
/// o como libro de Excel (ClosedXML).
/// </summary>
public class StatementExportService : IStatementExportService
{
    private const string Currency = "#,##0.00";

    // Resumen del estado de cuenta con etiquetas en español (compartido por PDF y Excel).
    private static (string Label, decimal Value)[] BuildSummary(Statement statement) => new[]
    {
        ("Saldo Acumulado", statement.CurrentBalance),
        ("Límite de la Tarjeta", statement.CardLimit),
        ("Saldo Disponible", statement.AvailableBalance),
        ("Compras del Mes Actual", statement.CurrentMonthPurchases),
        ("Compras del Mes Anterior", statement.PreviousMonthPurchases),
        ("Interés Bonificable", statement.BonusInterest),
        ("Pago Mínimo", statement.MinimumPayment),
        ("Pago de Contado", statement.TotalPayment),
        ("Pago más Intereses", statement.TotalPaymentWithInterest),
    };

    public byte[] BuildPdf(Statement statement)
    {
        var summary = BuildSummary(statement);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(header =>
                {
                    header.Item().Text("Estado de Cuenta").FontSize(20).Bold();
                    header.Item().Text($"Cliente: {statement.ClientName}");
                    header.Item().Text($"Tarjeta: {statement.CardNumber}");
                    header.Item().Text($"Generado: {DateTime.Now:yyyy-MM-dd HH:mm}")
                        .FontSize(8).FontColor(Colors.Grey.Medium);
                });

                page.Content().PaddingVertical(15).Column(content =>
                {
                    content.Spacing(15);

                    // Resumen
                    content.Item().Text("Resumen").FontSize(14).Bold();
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn();
                            cols.ConstantColumn(120);
                        });

                        foreach (var (label, value) in summary)
                        {
                            table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5).Text(label);
                            table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5).AlignRight().Text(value.ToString(Currency));
                        }
                    });

                    // Movimientos
                    content.Item().Text("Movimientos").FontSize(14).Bold();
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(110); // Fecha
                            cols.ConstantColumn(90);  // Monto
                            cols.RelativeColumn();     // Descripción
                            cols.ConstantColumn(90);  // Tipo
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Fecha").Bold();
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Monto").Bold();
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Descripción").Bold();
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tipo").Bold();
                        });

                        foreach (var m in statement.Movements)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5).Text(m.MovementDate.ToString("yyyy-MM-dd HH:mm"));
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5).AlignRight().Text(m.MovementAmount.ToString(Currency));
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5).Text(m.MovementDescription);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(5).Text(m.MovementType);
                        }
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    public byte[] BuildExcel(Statement statement)
    {
        using var workbook = new XLWorkbook();

        // ---- Hoja Resumen ----
        var summarySheet = workbook.Worksheets.Add("Resumen");
        summarySheet.Cell(1, 1).Value = "Estado de Cuenta";
        summarySheet.Cell(1, 1).Style.Font.Bold = true;
        summarySheet.Cell(1, 1).Style.Font.FontSize = 16;

        summarySheet.Cell(2, 1).Value = "Cliente";
        summarySheet.Cell(2, 2).Value = statement.ClientName;
        summarySheet.Cell(3, 1).Value = "Tarjeta";
        summarySheet.Cell(3, 2).Value = statement.CardNumber;
        summarySheet.Cell(4, 1).Value = "Generado";
        summarySheet.Cell(4, 2).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        var summary = BuildSummary(statement);

        var row = 6;
        foreach (var (label, value) in summary)
        {
            summarySheet.Cell(row, 1).Value = label;
            summarySheet.Cell(row, 1).Style.Font.Bold = true;
            var valueCell = summarySheet.Cell(row, 2);
            valueCell.Value = value;
            valueCell.Style.NumberFormat.Format = Currency;
            row++;
        }

        summarySheet.Columns().AdjustToContents();

        // ---- Hoja Movimientos ----
        var movementsSheet = workbook.Worksheets.Add("Movimientos");
        var headers = new[] { "Fecha", "Monto", "Descripción", "Tipo" };
        for (var c = 0; c < headers.Length; c++)
        {
            var cell = movementsSheet.Cell(1, c + 1);
            cell.Value = headers[c];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        var r = 2;
        foreach (var m in statement.Movements)
        {
            movementsSheet.Cell(r, 1).Value = m.MovementDate;
            movementsSheet.Cell(r, 1).Style.DateFormat.Format = "yyyy-mm-dd hh:mm";
            var amountCell = movementsSheet.Cell(r, 2);
            amountCell.Value = m.MovementAmount;
            amountCell.Style.NumberFormat.Format = Currency;
            movementsSheet.Cell(r, 3).Value = m.MovementDescription;
            movementsSheet.Cell(r, 4).Value = m.MovementType;
            r++;
        }

        movementsSheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
