using System.ComponentModel;

namespace AISchool.Utils
{
	public class SortableBindingList<T> : BindingList<T>
	{
		private bool _isSorted;
		private ListSortDirection _sortDirection;
		private PropertyDescriptor? _sortProperty;

		public SortableBindingList(IList<T> list) : base(list) { }

		protected override bool SupportsSortingCore => true;
		protected override bool IsSortedCore => _isSorted;
		protected override PropertyDescriptor? SortPropertyCore => _sortProperty;
		protected override ListSortDirection SortDirectionCore => _sortDirection;

		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			if (Items is not List<T> items) return;

			items.Sort((a, b) =>
			{
				var valueA = prop.GetValue(a);
				var valueB = prop.GetValue(b);

				int result;
				if (valueA == null)
				{
					result = (valueB == null) ? 0 : -1;
				}
				else if (valueB == null)
				{
					result = 1;
				}
				else if (valueA is IComparable comparableA)
				{
					result = comparableA.CompareTo(valueB);
				}
				else
				{
					result = valueA.ToString()!.CompareTo(valueB.ToString());
				}

				if (direction == ListSortDirection.Descending)
				{
					result = -result;
				}

				return result;
			});

			_isSorted = true;
			_sortProperty = prop;
			_sortDirection = direction;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		protected override void RemoveSortCore()
		{
			_isSorted = false;
			_sortProperty = null;
		}
	}
}