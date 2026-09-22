namespace EA.Iws.Web.Tests.Unit.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using EA.Iws.Web.ViewModels.Shared;
    using Xunit;

    public class CheckBoxCollectionViewModelTests
    {
        private enum TestColumns
        {
            [Display(Name = "First Column")]
            FirstColumn = 5,

            [Display(Name = "Second Column")]
            SecondColumn = 12,

            ThirdColumn = 99
        }

        [Fact]
        public void CreateFromEnum_SetsValueToEnumMemberName_NotIntegerValue()
        {
            var model = CheckBoxCollectionViewModel.CreateFromEnum<TestColumns>();

            var values = model.PossibleValues.Select(p => p.Value).ToList();

            Assert.Contains(nameof(TestColumns.FirstColumn), values);
            Assert.Contains(nameof(TestColumns.SecondColumn), values);
            Assert.Contains(nameof(TestColumns.ThirdColumn), values);

            // Ensure it is not using the underlying integer values.
            Assert.DoesNotContain("5", values);
            Assert.DoesNotContain("12", values);
            Assert.DoesNotContain("99", values);
        }

        [Fact]
        public void CreateFromEnum_UsesDisplayNameAsText_WhenPresent()
        {
            var model = CheckBoxCollectionViewModel.CreateFromEnum<TestColumns>();

            var firstColumn = model.PossibleValues.Single(p => p.Value == nameof(TestColumns.FirstColumn));

            Assert.Equal("First Column", firstColumn.Text);
        }

        [Fact]
        public void CreateFromEnum_UsesFieldName_WhenNoDisplayAttribute()
        {
            var model = CheckBoxCollectionViewModel.CreateFromEnum<TestColumns>();

            var thirdColumn = model.PossibleValues.Single(p => p.Value == nameof(TestColumns.ThirdColumn));

            Assert.Equal(nameof(TestColumns.ThirdColumn), thirdColumn.Text);
        }

        [Fact]
        public void CreateFromEnum_AllItemsDefaultToUnselected()
        {
            var model = CheckBoxCollectionViewModel.CreateFromEnum<TestColumns>();

            Assert.All(model.PossibleValues, item => Assert.False(item.Selected));
        }

        [Fact]
        public void SetSelectedValues_Generic_SelectsMatchingItemsByName()
        {
            var model = CheckBoxCollectionViewModel.CreateFromEnum<TestColumns>();

            model.SetSelectedValues(new[] { TestColumns.SecondColumn });

            var selected = model.PossibleValues.Where(p => p.Selected).ToList();

            Assert.Single(selected);
            Assert.Equal(nameof(TestColumns.SecondColumn), selected[0].Value);
        }

        [Fact]
        public void SetSelectedValues_Generic_DoesNotSelectUnrelatedItems()
        {
            var model = CheckBoxCollectionViewModel.CreateFromEnum<TestColumns>();

            model.SetSelectedValues(new[] { TestColumns.FirstColumn });

            var unselected = model.PossibleValues.Where(p => !p.Selected).Select(p => p.Value).ToList();

            Assert.Contains(nameof(TestColumns.SecondColumn), unselected);
            Assert.Contains(nameof(TestColumns.ThirdColumn), unselected);
        }
    }
}