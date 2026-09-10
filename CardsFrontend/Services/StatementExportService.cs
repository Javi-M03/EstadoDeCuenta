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

public class StatementExportService : IStatementExportService
{
    private const string Currency = "#,##0.00";

    public byte[] BuildPdf(Statement statement)
    {
        var summary = new (string Label, decimal Value)[]
        {
            ("Current Balance", statement.CurrentBalance),
            ("Card Limit", statement.CardLimit),
            ("Available Balance", statement.AvailableBalance),
            ("Current Month Purchases", statement.CurrentMonthPurchases),
            ("Previous Month Purchases", statement.PreviousMonthPurchases),
            ("Bonus Interest", statement.BonusInterest),
            ("Minimum Payment", statement.MinimumPayment),
            ("Total Payment", statement.TotalPayment),
            ("Total Payment With Interest", statement.TotalPaymentWithInterest),
        };

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(header =>
                {
                    header.Item().Text("Card Statement").FontSize(20).Bold();
                    header.Item().Text($"Client: {statement.ClientName}");
                    header.Item().Text($"Card: {statement.CardNumber}");
                    header.Item().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}")
                        .FontSize(8).FontColor(Colors.Grey.Medium);
                });

                page.Content().PaddingVertical(15).Column(content =>
                {
                    content.Spacing(15);

                    // Summary
                    content.Item().Text("Summary").FontSize(14).Bold();
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

                    // Movements
                    content.Item().Text("Movements").FontSize(14).Bold();
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(110); // Date
                            cols.ConstantColumn(90);  // Amount
                            cols.RelativeColumn();     // Description
                            cols.ConstantColumn(90);  // Type
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Date").Bold();
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Amount").Bold();
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Description").Bold();
                            h.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Type").Bold();
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

        var summarySheet = workbook.Worksheets.Add("Summary");
        summarySheet.Cell(1, 1).Value = "Card Statement";
        summarySheet.Cell(1, 1).Style.Font.Bold = true;
        summarySheet.Cell(1, 1).Style.Font.FontSize = 16;

        summarySheet.Cell(2, 1).Value = "Client";
        summarySheet.Cell(2, 2).Value = statement.ClientName;
        summarySheet.Cell(3, 1).Value = "Card";
        summarySheet.Cell(3, 2).Value = statement.CardNumber;
        summarySheet.Cell(4, 1).Value = "Generated";
        summarySheet.Cell(4, 2).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        var summary = new (string Label, decimal Value)[]
        {
            ("Current Balance", statement.CurrentBalance),
            ("Card Limit", statement.CardLimit),
            ("Available Balance", statement.AvailableBalance),
            ("Current Month Purchases", statement.CurrentMonthPurchases),
            ("Previous Month Purchases", statement.PreviousMonthPurchases),
            ("Bonus Interest", statement.BonusInterest),
            ("Minimum Payment", statement.MinimumPayment),
            ("Total Payment", statement.TotalPayment),
            ("Total Payment With Interest", statement.TotalPaymentWithInterest),
        };

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

        var movementsSheet = workbook.Worksheets.Add("Movements");
        var headers = new[] { "Date", "Amount", "Description", "Type" };
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
