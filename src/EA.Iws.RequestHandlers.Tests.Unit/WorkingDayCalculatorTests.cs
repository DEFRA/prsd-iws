// ReSharper disable InconsistentNaming

namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using DataAccess;
    using Domain;
    using TestHelpers.Helpers;
    using Xunit;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class WorkingDayCalculatorTests
    {
        private static readonly DateTime Wednesday1stJuly2015 = new DateTime(2015, 7, 1);
        private static readonly DateTime Thursday2ndJuly2015 = new DateTime(2015, 7, 2);
        private static readonly DateTime Friday3rdJuly2015 = new DateTime(2015, 7, 3);
        private static readonly DateTime Saturday4thJuly = new DateTime(2015, 7, 4);
        private static readonly DateTime Sunday5thJuly2015 = new DateTime(2015, 7, 5);
        private static readonly DateTime Monday6thJuly2015 = new DateTime(2015, 7, 6);
        private static readonly DateTime Tuesday7thJuly2015 = new DateTime(2015, 7, 7);
        private static readonly DateTime Wednesday8thJuly2015 = new DateTime(2015, 7, 8);
        private static readonly DateTime Thursday9thJuly2015 = new DateTime(2015, 7, 9);
        private static readonly DateTime Friday10thJuly2015 = new DateTime(2015, 7, 10);
        private static readonly DateTime Saturday11thJuly2015 = new DateTime(2015, 7, 11);
        private static readonly DateTime Sunday12thJuly2015 = new DateTime(2015, 7, 12);
        private static readonly DateTime Monday13thJuly2015 = new DateTime(2015, 7, 13);
        private static readonly DateTime Tuesday14thJuly2015BankHoliday = new DateTime(2015, 7, 14);
        private static readonly DateTime Wednesday15thJuly2015 = new DateTime(2015, 7, 15);
        private static readonly DateTime Thursday16thJuly2015 = new DateTime(2015, 7, 16);
        private static readonly DateTime Friday17thJuly2015 = new DateTime(2015, 7, 17);
        private static readonly DateTime Saturday18thJuly2015 = new DateTime(2015, 7, 18);
        private static readonly DateTime Sunday19thJuly2015 = new DateTime(2015, 7, 19);
        private static readonly DateTime Monday20thJuly2015 = new DateTime(2015, 7, 20);
        private static readonly DateTime Tuesday21stJuly2015 = new DateTime(2015, 7, 21);

        private static readonly DateTime Thursday23rdJuly2015 = new DateTime(2015, 7, 23);
        private static readonly DateTime Friday24thJuly2015BankHoliday = new DateTime(2015, 7, 24);
        private static readonly DateTime Monday27thJuly2015 = new DateTime(2015, 7, 27);

        private readonly WorkingDayCalculator calculator;
        private readonly IwsContext context;

        public WorkingDayCalculatorTests()
        {
            context = new TestIwsContext();

            var bankHoliday = ObjectInstantiator<BankHoliday>.CreateNew();
            ObjectInstantiator<BankHoliday>.SetProperty(bh => bh.CompetentAuthority, CompetentAuthorityEnum.England,
                bankHoliday);
            ObjectInstantiator<BankHoliday>.SetProperty(bh => bh.Date, Tuesday14thJuly2015BankHoliday, bankHoliday);

            var secondBankHoliday = ObjectInstantiator<BankHoliday>.CreateNew();
            ObjectInstantiator<BankHoliday>.SetProperty(bh => bh.CompetentAuthority, CompetentAuthorityEnum.England, 
                secondBankHoliday);
            ObjectInstantiator<BankHoliday>.SetProperty(bh => bh.Date, Friday24thJuly2015BankHoliday, secondBankHoliday);

            context.BankHolidays.AddRange(new[] { bankHoliday, secondBankHoliday });

            calculator = new WorkingDayCalculator(context);
        }

        public static IEnumerable<object[]> GetWorkingDaysData
        {
            get
            {
                return new[]
                {
                    // Test positive week days.
                    new object[] { Wednesday1stJuly2015, Thursday2ndJuly2015, false, 1 },
                    new object[] { Wednesday1stJuly2015, Thursday2ndJuly2015, true, 2 },
                    new object[] { Wednesday1stJuly2015, Friday3rdJuly2015, true, 3 },
                    new object[] { Wednesday1stJuly2015, Friday3rdJuly2015, false, 2 },

                    // Test negative week days.
                    new object[] { Thursday2ndJuly2015, Wednesday1stJuly2015, false, -1 },
                    new object[] { Thursday2ndJuly2015, Wednesday1stJuly2015, true, -2 },

                    // Test same day.
                    new object[] { Wednesday1stJuly2015, Wednesday1stJuly2015, false, 0 },
                    new object[] { Wednesday1stJuly2015, Wednesday1stJuly2015, true, 1 },

                    // Test day including Saturday.
                    new object[] { Wednesday1stJuly2015, Saturday4thJuly, false, 2 },
                    new object[] { Wednesday1stJuly2015, Saturday4thJuly, true, 3 },
                    new object[] { Thursday2ndJuly2015, Saturday4thJuly, false, 1 },
                    new object[] { Thursday2ndJuly2015, Saturday4thJuly, true, 2 },
                    new object[] { Saturday4thJuly, Saturday4thJuly, false, 0 },
                    new object[] { Saturday4thJuly, Saturday4thJuly, true, 0 },

                    // Test day including Saturday but negative timespan.
                    new object[] { Saturday4thJuly, Wednesday1stJuly2015, false, -3 },
                    new object[] { Saturday4thJuly, Wednesday1stJuly2015, true, -3 },

                    // Test day including Sunday but negative timespan.
                    new object[] { Sunday5thJuly2015, Friday3rdJuly2015, true, -1 },
                    new object[] { Sunday5thJuly2015, Friday3rdJuly2015, false, -1 },
                    new object[] { Sunday5thJuly2015, Thursday2ndJuly2015, true, -2 },
                    new object[] { Sunday5thJuly2015, Thursday2ndJuly2015, false, -2 },

                    // Test day including Sunday.
                    new object[] { Sunday5thJuly2015, Monday6thJuly2015, false, 1 },
                    new object[] { Sunday5thJuly2015, Monday6thJuly2015, true, 1 },
                    new object[] { Sunday5thJuly2015, Tuesday7thJuly2015, false, 2 },
                    new object[] { Sunday5thJuly2015, Tuesday7thJuly2015, true, 2 },
                    new object[] { Sunday5thJuly2015, Sunday5thJuly2015, false, 0 },
                    new object[] { Sunday5thJuly2015, Sunday5thJuly2015, true, 0 },

                    // Test day including weekend.
                    new object[] { Wednesday1stJuly2015, Monday6thJuly2015, false, 3 },
                    new object[] { Wednesday1stJuly2015, Monday6thJuly2015, true, 4 },
                    new object[] { Wednesday1stJuly2015, Tuesday7thJuly2015, false, 4 },
                    new object[] { Wednesday1stJuly2015, Tuesday7thJuly2015, true, 5 },
                    new object[] { Wednesday1stJuly2015, Friday10thJuly2015, false, 7 },
                    new object[] { Wednesday1stJuly2015, Friday10thJuly2015, true, 8 },

                    // Test day including weekend but negative timespan.
                    new object[] { Monday6thJuly2015, Wednesday1stJuly2015, false, -3 },
                    new object[] { Monday6thJuly2015, Wednesday1stJuly2015, true, -4 },
                    new object[] { Tuesday7thJuly2015, Wednesday1stJuly2015, false, -4 },
                    new object[] { Tuesday7thJuly2015, Wednesday1stJuly2015, true, -5 },

                    // Weekend to Weekend.
                    new object[] { Saturday4thJuly, Saturday11thJuly2015, false, 5 },
                    new object[] { Saturday4thJuly, Saturday11thJuly2015, true, 5 },
                    new object[] { Sunday5thJuly2015, Saturday11thJuly2015, false, 5 },
                    new object[] { Sunday5thJuly2015, Saturday11thJuly2015, true, 5 },
                    new object[] { Sunday5thJuly2015, Sunday12thJuly2015, false, 5 },
                    new object[] { Sunday5thJuly2015, Sunday12thJuly2015, true, 5 },
                    new object[] { Saturday4thJuly, Sunday12thJuly2015, false, 5 },
                    new object[] { Saturday4thJuly, Sunday12thJuly2015, true, 5 },

                    // Weekend to weekend but negative timespan.
                    new object[] { Saturday11thJuly2015, Saturday4thJuly, false, -5 },
                    new object[] { Saturday11thJuly2015, Saturday4thJuly, true, -5 },
                    new object[] { Saturday11thJuly2015, Sunday5thJuly2015, false, -5 },
                    new object[] { Saturday11thJuly2015, Sunday5thJuly2015, true, -5 },
                    new object[] { Sunday12thJuly2015, Sunday5thJuly2015, false, -5 },
                    new object[] { Sunday12thJuly2015, Sunday5thJuly2015, true, -5 },
                    new object[] { Sunday12thJuly2015, Saturday4thJuly, false, -5 },
                    new object[] { Sunday12thJuly2015, Saturday4thJuly, true, -5 },

                    // Timespan including bank holiday.
                    new object[] { Monday13thJuly2015, Wednesday15thJuly2015, false, 1 }, 
                    new object[] { Monday13thJuly2015, Wednesday15thJuly2015, true, 2 }, 
                    new object[] { Monday13thJuly2015, Thursday16thJuly2015, false, 2 }, 
                    new object[] { Monday13thJuly2015, Thursday16thJuly2015, true, 3 },
 
                    // Timespan including bank holiday but negative.
                    new object[] { Wednesday15thJuly2015, Monday13thJuly2015, false, -1 }, 
                    new object[] { Wednesday15thJuly2015, Monday13thJuly2015, true, -2 }, 
                    new object[] { Thursday16thJuly2015, Monday13thJuly2015, false, -2 }, 
                    new object[] { Thursday16thJuly2015, Monday13thJuly2015, true, -3 },

                    // Timespan starts on bank holiday.
                    new object[] { Tuesday14thJuly2015BankHoliday, Wednesday15thJuly2015, false, 1 }, 
                    new object[] { Tuesday14thJuly2015BankHoliday, Wednesday15thJuly2015, true, 1 }, 
                    new object[] { Tuesday14thJuly2015BankHoliday, Thursday16thJuly2015, false, 2 }, 
                    new object[] { Tuesday14thJuly2015BankHoliday, Thursday16thJuly2015, true, 2 },

                    // Timespan starts on bank holiday but negative.
                    new object[] { Tuesday14thJuly2015BankHoliday, Monday13thJuly2015, false, -1 }, 
                    new object[] { Tuesday14thJuly2015BankHoliday, Monday13thJuly2015, true, -1 }, 
                    new object[] { Tuesday14thJuly2015BankHoliday, Friday10thJuly2015, false, -2 }, 
                    new object[] { Tuesday14thJuly2015BankHoliday, Friday10thJuly2015, true, -2 },

                    // Longer time-spans.
                    new object[] { Wednesday8thJuly2015, Tuesday21stJuly2015, false, 8 }, 
                    new object[] { Wednesday8thJuly2015, Tuesday21stJuly2015, true, 9 }, 
                    new object[] { Thursday9thJuly2015, Monday20thJuly2015, false, 6 }, 
                    new object[] { Friday17thJuly2015, Monday13thJuly2015, true, -4 },
                    new object[] { Saturday18thJuly2015, Wednesday1stJuly2015, false, -12 },
                    new object[] { Sunday19thJuly2015, Saturday4thJuly, true, -9 },
                };
            }
        }

        public static IEnumerable<object[]> AddWorkingDayData
        {
            get
            {
                return new[]
                {
                    // Test in the same week.
                    new object[] { Wednesday1stJuly2015, 1, false, Thursday2ndJuly2015 },
                    new object[] { Wednesday1stJuly2015, 1, true, Wednesday1stJuly2015 },
                    new object[] { Wednesday1stJuly2015, 2, false, Friday3rdJuly2015 },
                    new object[] { Wednesday1stJuly2015, 2, true, Thursday2ndJuly2015 },
                    new object[] { Wednesday1stJuly2015, 3, true, Friday3rdJuly2015 },

                    // Test overflowing a weekend.
                    new object[] { Wednesday1stJuly2015, 3, false, Monday6thJuly2015 },

                    // Test starting on a weekend.
                    new object[] { Saturday4thJuly, 1, true, Monday6thJuly2015 },
                    new object[] { Saturday4thJuly, 1, false, Monday6thJuly2015 },
                    new object[] { Sunday5thJuly2015, 1, false, Monday6thJuly2015 },
                    new object[] { Sunday5thJuly2015, 1, true, Monday6thJuly2015 },
                    new object[] { Sunday5thJuly2015, 5, false, Friday10thJuly2015 },
                    new object[] { Sunday5thJuly2015, 6, false, Monday13thJuly2015 },

                    // Test zero.
                    new object[] { Tuesday7thJuly2015, 0, true, Tuesday7thJuly2015 },
                    new object[] { Tuesday7thJuly2015, 0, false, Tuesday7thJuly2015 },

                    // Test starts on bank holidays.
                    new object[] { Tuesday14thJuly2015BankHoliday, 1, false, Wednesday15thJuly2015 },
                    new object[] { Tuesday14thJuly2015BankHoliday, 1, true, Wednesday15thJuly2015 },
                    new object[] { Tuesday14thJuly2015BankHoliday, 2, false, Thursday16thJuly2015 },
                    new object[] { Tuesday14thJuly2015BankHoliday, 2, true, Thursday16thJuly2015 },
                    new object[] { Tuesday14thJuly2015BankHoliday, 3, true, Friday17thJuly2015 },

                    // Test negative days.
                    new object[] { Friday3rdJuly2015, -1, false, Thursday2ndJuly2015 }, 
                    new object[] { Friday3rdJuly2015, -1, true, Friday3rdJuly2015 }, 
                    new object[] { Friday3rdJuly2015, -2, false, Wednesday1stJuly2015 }, 
                    new object[] { Friday3rdJuly2015, -2, true, Thursday2ndJuly2015 }, 

                    // Test includes bank holiday.
                    new object[] { Monday13thJuly2015, 1, false, Wednesday15thJuly2015 }, 
                    new object[] { Monday13thJuly2015, 1, true, Monday13thJuly2015 }, 
                    new object[] { Monday13thJuly2015, 2, false, Thursday16thJuly2015 }, 
                    new object[] { Sunday12thJuly2015, 2, false, Wednesday15thJuly2015 }, 
                
                    // Test negative bank holiday.
                    new object[] { Wednesday15thJuly2015, -1, false, Monday13thJuly2015 }, 
                    new object[] { Wednesday15thJuly2015, -2, false, Friday10thJuly2015 }, 

                    // Test with bank holiday adjacent to weekend.
                    new object[] { Thursday23rdJuly2015, 1, false, Monday27thJuly2015 }, 
                    new object[] { Friday24thJuly2015BankHoliday, 1, true, Monday27thJuly2015 }, 
                };
            }
        }

        [Theory, MemberData("GetWorkingDaysData")]
        public void GetWorkingDaysReturnsExpectedResult(DateTime start, DateTime end, bool includeStart,
            int expectedDays)
        {
            var result = calculator.GetWorkingDays(start, end, includeStart);

            Assert.Equal(expectedDays, result);
        }

        [Theory, MemberData("AddWorkingDayData")]
        public void AddWorkingDaysReturnsExpectedResult(DateTime start, int days, bool includeStart,
            DateTime expectedResult)
        {
            var result = calculator.AddWorkingDays(start, days, includeStart);

            Assert.Equal(expectedResult, result);
        }
    }
}