# Querying in Entity Framework Core

If you are new to EF-Core and you don't know how to convert your existing SQL queries to LINQ to entities queries, these exercises will help you to get started.

These samples are ported from [PostgreSQL Exercises](https://pgexercises.com/) to EF-Core.

![Querying in Entity Framework Core](/App_Data/EFCorePgExercises.png)

## [Simple SQL Queries](/Exercises/SimpleSQLQueries)

- [Retrieve everything from a table](/Exercises/SimpleSQLQueries/RetrieveEverythingFromATable.cs)
- [Retrieve specific columns from a table](/Exercises/SimpleSQLQueries/RetrieveSpecificColumnsFromATable.cs)
- [Control which rows are retrieved](/Exercises/SimpleSQLQueries/ControlWhichRowsAreRetrieved.cs)
- [Control which rows are retrieved - part 2](/Exercises/SimpleSQLQueries/ControlWhichRowsAreRetrievedPart2.cs)
- [Basic string searches](/Exercises/SimpleSQLQueries/BasicStringSearches.cs)
- [Matching against multiple possible values](/Exercises/SimpleSQLQueries/MatchingAgainstMultiplePossibleValues.cs)
- [Classify results into buckets](/Exercises/SimpleSQLQueries/ClassifyResultsIntoBuckets.cs)
- [Working with dates](/Exercises/SimpleSQLQueries/WorkingWithDates.cs)
- [Removing duplicates, and ordering results](/Exercises/SimpleSQLQueries/RemovingDuplicatesAndOrderingResults.cs)
- [Combining results from multiple queries](/Exercises/SimpleSQLQueries/CombiningResultsFromMultipleQueries.cs)
- [Simple aggregation](/Exercises/SimpleSQLQueries/SimpleAggregation.cs)
- [More aggregation](/Exercises/SimpleSQLQueries/MoreAggregation.cs)
- [Produce a list of facilities that are booked only in 2012](/Exercises/SimpleSQLQueries/ListOfFacilitiesThatAreBookedOnlyIn2012.cs)

## [Joins and Sub-queries](/Exercises/JoinsAndSubqueries)

- [Retrieve the start times of members' bookings](/Exercises/JoinsAndSubqueries/RetrieveTheStartTimesOfMembersBookings.cs)
- [Work out the start times of bookings for tennis courts](/Exercises/JoinsAndSubqueries/WorkoutTheStartTimesOfBookingsForTennisCourts.cs)
- [Produce a list of all members who have recommended another member](/Exercises/JoinsAndSubqueries/ProduceListOfAllMembersWhoHaveRecommendedAnotherMember.cs)
- [Produce a list of all members, along with their recommender](/Exercises/JoinsAndSubqueries/ProduceListOfAllMembersAlongWithTheirRecommender)
- [Produce a list of all members who have used a tennis court](/Exercises/JoinsAndSubqueries/ProduceListOfAllMembersWhoHaveUsedTennisCourt.cs)
- [Produce a list of costly bookings](/Exercises/JoinsAndSubqueries/ProduceListOfCostlyBookings.cs)
- [Produce a list of all members, along with their recommender, using no joins.](/Exercises/JoinsAndSubqueries/ProduceListOfAllMembersAlongWithTheirRecommenderUsingNoJoins.cs)
- [Produce a list of costly bookings, using a sub-query](/Exercises/JoinsAndSubqueries/ProduceListOfCostlyBookingsUsingSubquery.cs)
- [Produce a list of facilities which are not booked in 2013](/Exercises/JoinsAndSubqueries/ListOfFacilitiesWhichAreNotBookedIn2013.cs)

## [Modifying data](/Exercises/ModifyingData)

- [Insert some data into a table](/Exercises/ModifyingData/InsertSomeDataIntoATable.cs)
- [Insert multiple rows of data into a table](/Exercises/ModifyingData/InsertMultipleRowsOfDataIntoATable.cs)
- [Insert calculated data into a table](/Exercises/ModifyingData/InsertCalculatedDataIntoATable.cs)
- [Update some existing data](/Exercises/ModifyingData/UpdateSomeExistingData.cs)
- [Update multiple rows and columns at the same time](/Exercises/ModifyingData/UpdateMultipleRowsAndColumnsAtTheSameTime.cs)
- [Update a row based on the contents of another row](/Exercises/ModifyingData/UpdateARowBasedOnTheContentsOfAnotherRow.cs)
- [Delete all bookings](/Exercises/ModifyingData/DeleteAllBookings.cs)
- [Delete a member from the cd.members table](/Exercises/ModifyingData/DeleteAMemberFromTheMembersTable.cs)
- [Delete based on a sub-query](/Exercises/ModifyingData/DeleteBasedOnASubquery.cs)

## [Aggregation](/Exercises/Aggregation)

- [Count the number of facilities](/Exercises/Aggregation/CountTheNumberOfFacilities.cs)
- [Count the number of expensive facilities](/Exercises/Aggregation/CountTheNumberOfExpensiveFacilities.cs)
- [Count the number of recommendations each member makes.](/Exercises/Aggregation/CountTheNumberOfRecommendationsEachMemberMakes.cs)
- [List the total slots booked per facility](/Exercises/Aggregation/ListTheTotalSlotsBookedPerFacility.cs)
- [List the total slots booked per facility in a given month](/Exercises/Aggregation/ListTheTotalSlotsBookedPerFacilityInGivenMonth.cs)
- [List the total slots booked per facility per month](/Exercises/Aggregation/ListTheTotalSlotsBookedPerFacilityPerMonth.cs)
- [Find the count of members who have made at least one booking](/Exercises/Aggregation/FindTheCountOfMembersWhoHaveMadeAtLeastOneBooking.cs)
- [List facilities with more than 1000 slots booked](/Exercises/Aggregation/ListFacilitiesWithMoreThan1000SlotsBooked.cs)
- [Find the total revenue of each facility](/Exercises/Aggregation/FindTheTotalRevenueOfEachFacility.cs)
- [Find facilities with a total revenue less than 1000](/Exercises/Aggregation/FindFacilitiesWithTotalRevenueLessThan1000.cs)
- [Output the facility id that has the highest number of slots booked](/Exercises/Aggregation/OutputTheFacilityIdThatHasTheHighestNumberOfSlotsBooked.cs)
- [List the total slots booked per facility per month, part 2](/Exercises/Aggregation/ListTheTotalSlotsBookedPerFacilityPerMonthPart2.cs)
- [List the total hours booked per named facility](/Exercises/Aggregation/ListTheTotalHoursBookedPerNamedFacility.cs)
- [List each member's first booking after September 1st 2012](/Exercises/Aggregation/ListEachMembersFirstBookingAfterSeptember1st2012.cs)
- [Produce a list of member names, with each row containing the total member count](/Exercises/Aggregation/ProduceListOfMemberNamesWithEachRowContainingTheTotalMemberCount.cs)
- [Produce a numbered list of members](/Exercises/Aggregation/ProduceNumberedListOfMembers.cs)
- [Output the facility id that has the highest number of slots booked, again](/Exercises/Aggregation/OutputTheFacilityIdThatHasTheHighestNumberOfSlotsBookedAgain.cs)
- [Rank members by (rounded) hours used](/Exercises/Aggregation/RankMembersByHoursUsed.cs)
- [Find the top three revenue generating facilities](/Exercises/Aggregation/FindTheTopThreeRevenueGeneratingFacilities.cs)
- [Classify facilities by value](/Exercises/Aggregation/ClassifyFacilitiesByValue.cs)
- [Calculate the payback time for each facility](/Exercises/Aggregation/CalculateThePaybackTimeForEachFacility.cs)
- [Calculate a rolling average of total revenue](/Exercises/Aggregation/CalculateRollingAverageOfTotalRevenue.cs)

## [Working with Timestamps](/Exercises/WorkingWithTimestamps)

- [Produce a timestamp for 1 a.m. on the 31st of August 2012](/Exercises/WorkingWithTimestamps/ProduceTimestampFor1AMOnThe31stOfAugust2012.cs)
- [Subtract timestamps from each other](/Exercises/WorkingWithTimestamps/SubtractTimestampsFromEachOther.cs)
- [Generate a list of all the dates in October 2012](/Exercises/WorkingWithTimestamps/GenerateListOfAllTheDatesInOctober2012.cs)
- [Get the day of the month from a timestamp](/Exercises/WorkingWithTimestamps/GetTheDayOfTheMonthFromTimestamp.cs)
- [Work out the number of seconds between timestamps](/Exercises/WorkingWithTimestamps/WorkoutTheNumberOfSecondsBetweenTimestamps.cs)
- [Work out the number of days in each month of 2012](/Exercises/WorkingWithTimestamps/WorkoutTheNumberOfDaysInEachMonthOf2012.cs)
- [Work out the number of days remaining in the month](/Exercises/WorkingWithTimestamps/WorkoutTheNumberOfDaysRemainingInTheMonth.cs)
- [Work out the end time of bookings](/Exercises/WorkingWithTimestamps/WorkoutTheEndTimeOfBookings.cs)
- [Return a count of bookings for each month](/Exercises/WorkingWithTimestamps/ReturnCountOfBookingsForEachMonth.cs)
- [Work out the utilization percentage for each facility by month](/Exercises/WorkingWithTimestamps/WorkoutTheUtilisationPercentageForEachFacilityByMonth.cs)
- [Find registered members in given months](/Exercises/WorkingWithTimestamps/FindRegisteredMembersInGivenMonths.cs)

## [String Operations](/Exercises/StringOperations)

- [Format the names of members](/Exercises/StringOperations/FormatNamesOfMembers.cs)
- [Find facilities by a name prefix](/Exercises/StringOperations/FindFacilitiesByNamePrefix.cs)
- [Perform a case-insensitive search](/Exercises/StringOperations/PerformCaseInsensitiveSearch.cs)
- [Find telephone numbers with parentheses](/Exercises/StringOperations/FindTelephoneNumbersWithParentheses.cs)
- [Pad zip codes with leading zeroes](/Exercises/StringOperations/PadZipCodesWithLeadingZeroes.cs)
- [Count the number of members whose surname starts with each letter of the alphabet](/Exercises/StringOperations/CountNumberOfMembersWhoseSurnameStartsWithEachLetterOfTheAlphabet.cs)
- [Clean up telephone numbers](/Exercises/StringOperations/CleanupTelephoneNumbers.cs)

## [Recursive Queries](/Exercises/RecursiveQueries)

- [Find the upward recommendation chain for member ID 27](/Exercises/RecursiveQueries/FindTheUpwardRecommendationChainForMemberID27.cs)
- [Find the downward recommendation chain for member ID 1](/Exercises/RecursiveQueries/FindTheDownwardRecommendationChainForMemberID1.cs)
- [Produce a CTE that can return the upward recommendation chain for any member](/Exercises/RecursiveQueries/ProduceCTEThatCanReturnTheUpwardRecommendationChainForAnyMember.cs)
