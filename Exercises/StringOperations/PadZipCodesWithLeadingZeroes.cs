using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class PadZipCodesWithLeadingZeroes
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/pad.html
            // The zip codes in our example dataset have had leading zeroes removed from them by
            // virtue of being stored as a numeric type. Retrieve all zip codes from the members table,
            // padding any zip codes less than 5 characters long with leading zeroes. Order by the
            // new zip code.
            //
            // select lpad(cast(zipcode as char(5)),5,'0') zip from cd.members order by zip

            EFServiceProvider.RunInContext(context =>
            {
                // Method - 1
                var members = context.Members
                                    .Select(member => new { ZipCode = Convert.ToString(member.ZipCode) })
                                    .OrderBy(m => m.ZipCode)
                                    .ToList();
                /*
                SELECT CONVERT(nvarchar(max), [m].[ZipCode]) AS [Zip]
                    FROM [Members] AS [m]
                    ORDER BY CONVERT(nvarchar(max), [m].[ZipCode])
                */
                // Now using LINQ to Objects
                members = members.Select(member => new { ZipCode = member.ZipCode.PadLeft(5, '0') })
                                                    .OrderBy(m => m.ZipCode)
                                                    .ToList();
                var expectedResult = new[]
                {
                    new { ZipCode = "00000" },
                    new { ZipCode = "00234" },
                    new { ZipCode = "00234" },
                    new { ZipCode = "04321" },
                    new { ZipCode = "04321" },
                    new { ZipCode = "10383" },
                    new { ZipCode = "11986" },
                    new { ZipCode = "23423" },
                    new { ZipCode = "28563" },
                    new { ZipCode = "33862" },
                    new { ZipCode = "34232" },
                    new { ZipCode = "43532" },
                    new { ZipCode = "43533" },
                    new { ZipCode = "45678" },
                    new { ZipCode = "52365" },
                    new { ZipCode = "54333" },
                    new { ZipCode = "56754" },
                    new { ZipCode = "57392" },
                    new { ZipCode = "58393" },
                    new { ZipCode = "64577" },
                    new { ZipCode = "65332" },
                    new { ZipCode = "65464" },
                    new { ZipCode = "66796" },
                    new { ZipCode = "68666" },
                    new { ZipCode = "69302" },
                    new { ZipCode = "75655" },
                    new { ZipCode = "78533" },
                    new { ZipCode = "80743" },
                    new { ZipCode = "84923" },
                    new { ZipCode = "87630" },
                    new { ZipCode = "97676" },
                };

                members.Should().BeEquivalentTo(expectedResult);

                // Method - 2, Using a custom DbFunction
                var newMembers = context.Members
                                        .Select(member => new
                                        {
                                            ZipCode =
                                                SqlDbFunctionsExtensions.SqlReplicate(
                                                    "0", 5 - Convert.ToString(member.ZipCode).Length)
                                                + member.ZipCode
                                        })
                        .OrderBy(m => m.ZipCode)
                        .ToList();
                /*
                    SELECT REPLICATE(N'0', 5 - CAST(LEN(CONVERT(nvarchar(max), [m].[ZipCode])) AS int)) + CAST([m].[ZipCode] AS nvarchar(max)) AS [Zip]
                    FROM [Members] AS [m]
                    ORDER BY REPLICATE(N'0', 5 - CAST(LEN(CONVERT(nvarchar(max), [m].[ZipCode])) AS int)) + CAST([m].[ZipCode] AS nvarchar(max))
                */
                newMembers.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}