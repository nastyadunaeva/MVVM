using System;
using Xunit;
using ViewModel;


namespace ViewModelTests
{
    public class UnitTest1
    {
        [Fact]
        public void HasUnsavedChangesTest()
        {
            MainViewModel model = new MainViewModel();
            model.AddDefaults();
            Assert.True(model.HasUnsavedChanges());
        }
        [Fact]
        public void AddTest()
        {
            MainViewModel model = new MainViewModel();
            Assert.Equal(0, model.Coll.Count);
            model.AddDefaultGrid();
            Assert.Equal(1, model.Coll.Count);
            model.AddDefaultCollection();
            Assert.Equal(2, model.Coll.Count);
        }
        [Fact]
        public void RemoveAllTest()
        {
            MainViewModel model = new MainViewModel();
            Assert.True(model.ItemIsNull());
            Assert.NotNull(model.Coll);
            model.AddDefaults();
            model.RemoveAll();
            Assert.Equal(0, model.Coll.Count);
        }
        [Fact]
        public void MaxDistTest()
        {
            MainViewModel model = new MainViewModel();
            model.AddDefaultGrid();
            Assert.True(model.Max_dist != 0);
            model.RemoveAt(0);
            Assert.Equal(0.0, model.Max_dist, 6);
        }
        [Fact]
        public void ErrorDataTest()
        {
            Item item = new Item((float)0.1, (float)0.1, -1.2);
            Assert.Equal("Error data", item.Error);
            Assert.Equal("Field must be positive", item["Field"]);
        }
    }
}
