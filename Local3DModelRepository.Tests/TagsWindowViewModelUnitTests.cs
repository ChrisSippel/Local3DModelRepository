using System.Linq;
using Local3DModelRepository.Models;
using Local3DModelRepository.ViewModels;
using Local3DModelRepository.UiTools;
using Moq;
using Xunit;

namespace Local3DModelRepository.Tests
{
    public sealed class TagsWindowViewModelUnitTests
    {
        private readonly MockRepository _mockRepository;

        public TagsWindowViewModelUnitTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
        }

        [Fact]
        public void Constructor_ValidateProperties()
        {
            var possibleTags = new Tag[] { new Tag("foo")};
            var modelTags = new Tag[] { new Tag("bar")};

            var viewModel = new TagsWindowViewModel(possibleTags, modelTags);
            Assert.Equal(possibleTags, viewModel.PossibleTags);
            Assert.Equal(modelTags, viewModel.SelectedTags);
            Assert.False(viewModel.SaveChanges);
            Assert.Equal(string.Empty, viewModel.UserAddedTags);
        }

        [Fact]
        public void CloseWithoutSavingCommand()
        {
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);

            var window = _mockRepository.Create<IClosableWindow>();
            window
                .Setup(x => x.Close());

            viewModel.CloseWithoutSavingCommand.Execute(window.Object);

            Assert.False(viewModel.SaveChanges);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void CloseAndSavingCommand()
        {
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);

            var window = _mockRepository.Create<IClosableWindow>();
            window
                .Setup(x => x.Close());

            viewModel.CloseAndSaveCommand.Execute(window.Object);

            Assert.True(viewModel.SaveChanges);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddSelectedTagCommand_NotATag()
        {
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);
            viewModel.AddSelectedTag.Execute(new object());
            Assert.Empty(viewModel.SelectedTags);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddSelectedTagCommand_ValidTag()
        {
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);

            var tagToAdd = new Tag("Hello");
            viewModel.AddSelectedTag.Execute(tagToAdd);
            Assert.Single(viewModel.SelectedTags);
            Assert.Contains(tagToAdd, viewModel.SelectedTags);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void RemoveSelectedTag_NotATag()
        {
            var tagToAdd = new Tag("Hello");
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[] { tagToAdd });

            viewModel.RemoveSelectedTag.Execute(new object());

            Assert.Single(viewModel.SelectedTags);
            Assert.Contains(tagToAdd, viewModel.SelectedTags);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void RemoveSelectedTag_DifferentTag()
        {
            var tagToAdd = new Tag("Hello");
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[] { tagToAdd });

            var tagToRemove = new Tag("World");
            viewModel.RemoveSelectedTag.Execute(tagToRemove);

            Assert.Single(viewModel.SelectedTags);
            Assert.Contains(tagToAdd, viewModel.SelectedTags);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void RemoveSelectedTag_ValidTag()
        {
            var tagToAddAndRemove = new Tag("Hello");
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[] { tagToAddAndRemove });

            viewModel.RemoveSelectedTag.Execute(tagToAddAndRemove);

            Assert.Empty(viewModel.SelectedTags);
            _mockRepository.VerifyAll(); _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddUserGivenTags_EmptyUserGivenTags()
        {
            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);
            viewModel.AddUserGivenTags.Execute(new object());

            Assert.Equal(string.Empty, viewModel.UserAddedTags);
            Assert.Empty(viewModel.SelectedTags);
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddUserGivenTags_SingleUserGivenTag()
        {
            const string expectedTagValue = "Hello";

            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);
            viewModel.UserAddedTags = expectedTagValue;
            viewModel.AddUserGivenTags.Execute(new object());

            Assert.Equal(string.Empty, viewModel.UserAddedTags);
            Assert.Single(viewModel.SelectedTags);

            Assert.NotNull(viewModel.SelectedTags.Single(x => x.Value == expectedTagValue));
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AddUserGivenTags_MultipleUserGivenTags()
        {
            const string expectedTagOneValue = "Hello";
            const string expectedTagTwoValue = "World";

            var viewModel = new TagsWindowViewModel(new Tag[0], new Tag[0]);
            viewModel.UserAddedTags = $"{expectedTagOneValue} {expectedTagTwoValue}";
            viewModel.AddUserGivenTags.Execute(new object());

            Assert.Equal(string.Empty, viewModel.UserAddedTags);
            Assert.Equal(2, viewModel.SelectedTags.Count);

            Assert.NotNull(viewModel.SelectedTags.Single(x => x.Value == expectedTagOneValue));
            Assert.NotNull(viewModel.SelectedTags.Single(x => x.Value == expectedTagTwoValue));
            _mockRepository.VerifyAll();
        }
    }
}
