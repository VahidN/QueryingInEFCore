using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCorePgExercises.Utils
{
    public enum SqlDateDiff
    {
        Year,
        Quarter,
        Month,
        DayOfYear,
        Day,
        Week,
        Hour,
        Minute,
        Second,
        MilliSecond,
        MicroSecond,
        NanoSecond
    }

    public enum SqlDatePart
    {
        Year,
        Quarter,
        Month,
        DayOfYear,
        Day,
        Week,
        WeekDay,
        Hour,
        Minute,
        Second,
        MilliSecond,
        MicroSecond,
        NanoSecond,
        TzOffset,
        Iso_Week
    }

    public static class SqlDbFunctionsExtensions
    {
        public static string SqlReplicate(string expression, int count)
            => throw new InvalidOperationException($"{nameof(SqlReplicate)} method cannot be called from the client side.");

        private static readonly MethodInfo _sqlReplicateMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlReplicate),
                new[] { typeof(string), typeof(int) }
            );

        public static decimal SqlRound(decimal value, int precision)
            => throw new InvalidOperationException($"{nameof(SqlRound)} method cannot be called from the client side.");

        private static readonly MethodInfo _sqlRoundMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlRound),
                new[] { typeof(decimal), typeof(int) }
            );

        public static int SqlDay(DateTime date)
            => throw new InvalidOperationException($"{nameof(SqlDay)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlDayMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlDay),
                new[] { typeof(DateTime) }
            );

        public static int SqlMonth(DateTime date)
            => throw new InvalidOperationException($"{nameof(SqlMonth)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlMonthMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlMonth),
                new[] { typeof(DateTime) }
            );

        public static int SqlYear(DateTime date)
            => throw new InvalidOperationException($"{nameof(SqlYear)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlYearMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlYear),
                new[] { typeof(DateTime) }
            );

        public static DateTime SqlGetDate()
            => throw new InvalidOperationException($"{nameof(SqlGetDate)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlGetDateMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlGetDate),
                new Type[] { }
            );

        public static DateTime SqlShortDate(DateTime Date)
            => throw new InvalidOperationException($"{nameof(SqlShortDate)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlShortDateMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlShortDate),
                new[] { typeof(DateTime) }
            );

        public static int SqlDateDiff(SqlDateDiff interval, DateTime initial, DateTime end)
            => throw new InvalidOperationException($"{nameof(SqlDateDiff)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlDateDiffMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlDateDiff),
                new[] { typeof(SqlDateDiff), typeof(DateTime), typeof(DateTime) }
            );

        public static int SqlDatePart(SqlDatePart interval, DateTime date)
            => throw new InvalidOperationException($"{nameof(SqlDatePart)} method cannot be called from the client side.");
        private static readonly MethodInfo _sqlDatePartMethodInfo = typeof(SqlDbFunctionsExtensions)
            .GetRuntimeMethod(
                nameof(SqlDbFunctionsExtensions.SqlDatePart),
                new[] { typeof(SqlDatePart), typeof(DateTime) }
            );


        public static void AddCustomSqlFunctions(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(_sqlReplicateMethodInfo)
                .HasTranslation(args =>
                {
                    return SqlFunctionExpression.Create("REPLICATE",
                        args,
                        _sqlReplicateMethodInfo.ReturnType,
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(_sqlRoundMethodInfo)
                .HasTranslation(args =>
                {
                    return SqlFunctionExpression.Create("ROUND",
                        args,
                        _sqlRoundMethodInfo.ReturnType,
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(_sqlDayMethodInfo)
                .HasTranslation(args => SqlFunctionExpression.Create(
                    "DAY",
                    args,
                    _sqlDayMethodInfo.ReturnType,
                    typeMapping: null));

            modelBuilder.HasDbFunction(_sqlMonthMethodInfo)
                .HasTranslation(args => SqlFunctionExpression.Create(
                    "MONTH",
                    args,
                    _sqlMonthMethodInfo.ReturnType,
                    typeMapping: null));

            modelBuilder.HasDbFunction(_sqlYearMethodInfo)
                .HasTranslation(args => SqlFunctionExpression.Create(
                    "YEAR",
                    args,
                    _sqlYearMethodInfo.ReturnType,
                    typeMapping: null));

            modelBuilder.HasDbFunction(_sqlGetDateMethodInfo)
                .HasTranslation(args => SqlFunctionExpression.Create(
                    "GETDATE",
                    args,
                    _sqlGetDateMethodInfo.ReturnType,
                    typeMapping: null));

            modelBuilder.HasDbFunction(_sqlShortDateMethodInfo)
                .HasTranslation(args => SqlFunctionExpression.Create(
                    "DBO.SHORTDATE",
                    args,
                    _sqlShortDateMethodInfo.ReturnType,
                    typeMapping: null));

            modelBuilder.HasDbFunction(_sqlDateDiffMethodInfo)
                .HasTranslation(args =>
                {
                    var parameters = args.ToArray();
                    var param0 = ((SqlConstantExpression)parameters[0]).Value.ToString();
                    return SqlFunctionExpression.Create("DATEDIFF",
                        new[]
                        {
                            new SqlFragmentExpression(param0), // It should be written as DateDiff(day, ...) and not DateDiff(N'day', ...) .
                            parameters[1],
                            parameters[2]
                        },
                        _sqlDateDiffMethodInfo.ReturnType,
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(_sqlDatePartMethodInfo)
                .HasTranslation(args =>
                {
                    var parameters = args.ToArray();
                    var param0 = ((SqlConstantExpression)parameters[0]).Value.ToString();
                    return SqlFunctionExpression.Create("DATEPART",
                        new[]
                        {
                            new SqlFragmentExpression(param0), // It should be written as DATEPART(day, ...) and not DATEPART(N'day', ...) .
                            parameters[1]
                        },
                        _sqlDatePartMethodInfo.ReturnType,
                        typeMapping: null);
                });
        }
    }
}