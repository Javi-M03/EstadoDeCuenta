using EstadoDeCuenta.Domain.Entities;
using EstadoDeCuenta.Domain.Enums;
using EstadoDeCuenta.DTOs.Statemets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstadoDeCuenta.Services.Services
{
    public class AccountStatementCalculator
    {
        public AccountStatementDto Calculate(
       Card card,
       IEnumerable<Movement> movements,
       decimal interestPercentage,
       decimal minimumPaymentPercentage)
        {
            var movementList = movements.ToList();

            var purchases = movementList
                .Where(m => m.MovementType == MovementTypeEnum.Compra)
                .ToList();

            var payments = movementList
                .Where(m => m.MovementType == MovementTypeEnum.Pago)
                .ToList();

            // Saldo Total = Compras - Pagos
            var currentBalance =
                purchases.Sum(m => m.MovementAmount)
                - payments.Sum(m => m.MovementAmount);

            // Saldo Disponible = Límite - Saldo Total
            var availableBalance =
                card.CardLimit - currentBalance;

            var today = DateTime.Now;

            // Inicio del mes actual
            var currentMonthStart = new DateTime(
                today.Year,
                today.Month,
                1);

            // Inicio del mes anterior
            var previousMonthStart = currentMonthStart.AddMonths(-1);

            var currentMonthPurchases = purchases
                .Where(m =>
                    m.MovementDate >= currentMonthStart &&
                    m.MovementDate < currentMonthStart.AddMonths(1))
                .Sum(m => m.MovementAmount);

            var previousMonthPurchases = purchases
                .Where(m =>
                    m.MovementDate >= previousMonthStart &&
                    m.MovementDate < currentMonthStart)
                .Sum(m => m.MovementAmount);

            // Convertimos porcentajes
            var interest = currentBalance * (interestPercentage / 100);

            var minimumPayment =
                currentBalance * (minimumPaymentPercentage / 100);

            var totalPayment = currentBalance;

            var totalPaymentWithInterest =
                currentBalance + interest;

            return new AccountStatementDto
            {
                ClientName = card.Client.ClientName,
                CardNumber = card.CardNumber,
                CurrentBalance = currentBalance,
                CardLimit = card.CardLimit,
                AvailableBalance = availableBalance,
                CurrentMonthPurchases = currentMonthPurchases,
                PreviousMonthPurchases = previousMonthPurchases,
                BonusInterest = interest,
                MinimumPayment = minimumPayment,
                TotalPayment = totalPayment,
                TotalPaymentWithInterest = totalPaymentWithInterest
            };
        }
    }
}
