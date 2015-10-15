using UnityEngine;
using System;
using System.Collections.Generic;

public class Sorts {

	public static List<DateTime> SortDateTimeAscending(List<DateTime> list) {
		list.Sort((a, b) => a.CompareTo(b));
		return list;
	}
	
	public static List<DateTime> SortDateTimeDescending(List<DateTime> list) {
		list.Sort((a, b) => b.CompareTo(a));
		return list;
	}

	public static List<TimeSpan> SortTimeSpanAscending(List<TimeSpan> list) {
		list.Sort((a, b) => a.CompareTo(b));
		return list;
	}
	
	public static List<TimeSpan> SortTimeSpanDescending(List<TimeSpan> list) {
		list.Sort((a, b) => b.CompareTo(a));
		return list;
	}

	public static List<TimeSpan> SortTimeNextInDay(List<TimeSpan> list) {
		list.Sort((a, b) => a.CompareTo(b));
		return list;
	}
}

// A comparison for times throughout a 24 hour period. Times of the day that have
// already past are considered to be 'later' than times that haven't occurred yet
// in the current 24 hour period.
// Ex: The current time is 11:15, and the list provided is 9:00, 11:00, 12:30, and 14:20.
//     The list post-sort would be: 12:30, 14:20, 9:00, and 11:00
public class TimeFromNowComparer : IComparer<DateTime>
{
	public TimeSpan timeOfDay = TimeSpan.MinValue;

	public int Compare(DateTime a, DateTime b)
	{
		if (timeOfDay.Equals(TimeSpan.MinValue)) {
			timeOfDay = DateTime.Now.TimeOfDay;
		}

		// If true, a is earlier in the day than now.
		if (timeOfDay.CompareTo(a.TimeOfDay) >= 0) {
			// If true, b is earlier in the day than now.
			if (timeOfDay.CompareTo(b.TimeOfDay) >= 0) {
				// Since they are both on the same side of 'now', sort normally.
				return a.TimeOfDay.CompareTo(b.TimeOfDay);
			} else { // b is later in the day than now.
				// a is earlier than b, but 'now' is past a, so treat b as less.
				return 1;
			}
		} else { // a is later in the day than now.
			// If true, b is earlier in the day than now.
			if (timeOfDay.CompareTo(b.TimeOfDay) >= 0) {
				// b is earlier than a, but 'now' is past b, so treat a as less.
				return -1;
			} else { // b is later in the day than now.
				// Since they are both on the same side of 'now', sort normally.
				return a.TimeOfDay.CompareTo(b.TimeOfDay);
			}
		}
		//return 0;
	}

	/*public int Compare(DateTime a, DateTime b)
	{
		// If true, a is earlier in the day than now.
		if (DateTime.Now.TimeOfDay.CompareTo(a.TimeOfDay) >= 0) {
			// If true, b is earlier in the day than now.
			if (DateTime.Now.TimeOfDay.CompareTo(b.TimeOfDay) >= 0) {
				// Since they are both on the same side of 'now', sort normally.
				return a.TimeOfDay.CompareTo(b.TimeOfDay);
			} else { // b is later in the day than now.
				// a is earlier than b, but 'now' is past a, so treat b as less.
				return 1;
			}
		} else { // a is later in the day than now.
			// If true, b is earlier in the day than now.
			if (DateTime.Now.TimeOfDay.CompareTo(b.TimeOfDay) >= 0) {
				// b is earlier than a, but 'now' is past b, so treat a as less.
				return -1;
			} else { // b is later in the day than now.
				// Since they are both on the same side of 'now', sort normally.
				return a.TimeOfDay.CompareTo(b.TimeOfDay);
			}
		}
		//return 0;
	}*/
}