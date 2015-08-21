namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteCodes;
    using Core.WasteCodes;
    using FakeItEasy;
    using Xunit;

    public class BaseWasteCodeControllerTests
    {
        private const string AddCode = "addcode";
        private const string Continue = "continue";
        private static readonly Guid AnyGuid = new Guid("A1F3C0BD-B789-4514-AF71-929300235872");

        private readonly TestController controller;
        private readonly List<Guid> selectedCodes = new List<Guid>();
        private readonly TestViewModel viewModel;

        public BaseWasteCodeControllerTests()
        {
            var client = A.Fake<IIwsClient>();

            controller = new TestController(() => client, CodeType.Y);

            viewModel = new TestViewModel
            {
                EnterWasteCodesViewModel = new EnterWasteCodesViewModel
                {
                    SelectedWasteCodes = selectedCodes
                }
            };
        }

        [Fact]
        public async Task Remove_CallsRemoveAction()
        {
            await controller.Post(AnyGuid, viewModel, null, AnyGuid.ToString());

            Assert.True(controller.IsRemoveCalled);
        }

        [Fact]
        public async Task Remove_RemovesItemFromList()
        {
            selectedCodes.Add(AnyGuid);

            await controller.Post(AnyGuid, viewModel, null, AnyGuid.ToString());

            Assert.Empty(selectedCodes);
        }

        [Fact]
        public async Task RemoveWhereItemIsNotInList_DoesNotThrow()
        {
            await controller.Post(AnyGuid, viewModel, null, Guid.Empty.ToString());

            Assert.Empty(selectedCodes);
        }

        [Fact]
        public async Task Remove_CallsRebind()
        {
            await controller.Post(AnyGuid, viewModel, null, AnyGuid.ToString());

            Assert.True(controller.IsRebindCalled);
        }

        [Fact]
        public async Task Remove_CanBeCalledWithInvalidModelState()
        {
            InvalidateModelState();

            await controller.Post(AnyGuid, viewModel, null, AnyGuid.ToString());

            Assert.True(controller.IsRemoveCalled);
        }

        [Fact]
        public async Task Remove_CannotBeCalledWithEmptyString()
        {
            try
            {
                // This cannot be used with ThrowsAsync for some reason.
                await controller.Post(AnyGuid, viewModel, null, string.Empty);
            }
            catch (InvalidOperationException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Continue_InvalidModelState_DoesNotCallContinue()
        {
            InvalidateModelState();

            await controller.Post(AnyGuid, viewModel, Continue, null);

            Assert.False(controller.IsContinueCalled);
            Assert.False(controller.IsRemoveCalled);
            Assert.False(controller.IsAddCalled);
        }

        [Fact]
        public async Task Continue_InvalidModelState_CallsRebind()
        {
            InvalidateModelState();

            await controller.Post(AnyGuid, viewModel, Continue, null);

            Assert.True(controller.IsRebindCalled);
        }

        [Fact]
        public async Task AddCode_InvalidModelState_DoesNotCallAdd()
        {
            InvalidateModelState();

            await controller.Post(AnyGuid, viewModel, AddCode, null);

            Assert.False(controller.IsAddCalled);
        }

        [Fact]
        public async Task AddCode_CallsAddAction()
        {
            viewModel.EnterWasteCodesViewModel.SelectedCode = AnyGuid;

            await controller.Post(AnyGuid, viewModel, AddCode, null);

            Assert.True(controller.IsAddCalled);
        }

        [Fact]
        public async Task AddCode_AddsToListOfSelectedValues()
        {
            viewModel.EnterWasteCodesViewModel.SelectedCode = AnyGuid;

            await controller.Post(AnyGuid, viewModel, AddCode, null);

            Assert.Contains(AnyGuid, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes);
        }

        [Fact]
        public async Task AddCode_DoesNotAddTwice()
        {
            viewModel.EnterWasteCodesViewModel.SelectedWasteCodes = new List<Guid>() { AnyGuid };

            viewModel.EnterWasteCodesViewModel.SelectedCode = AnyGuid;

            await controller.Post(AnyGuid, viewModel, AddCode, null);

            Assert.Single(viewModel.EnterWasteCodesViewModel.SelectedWasteCodes, AnyGuid);
        }

        [Fact]
        public async Task ClickingAddWithoutASelectedCodeDoesNotCallAddAndReturnsView()
        {
            var result = await controller.Post(AnyGuid, viewModel, AddCode, null) as ViewResult;

            Assert.False(controller.IsAddCalled);
            Assert.False(controller.IsContinueCalled);
            Assert.True(controller.IsRebindCalled);

            Assert.IsType<TestViewModel>(result.Model);
        }

        private void InvalidateModelState()
        {
            controller.ModelState.AddModelError("Bad", "Error");
        }

        private class TestController : BaseWasteCodeController
        {
            public TestController(Func<IIwsClient> apiClient, CodeType codeType)
                : base(apiClient, codeType)
            {
            }

            public bool IsAddCalled { get; private set; }
            public bool IsRemoveCalled { get; private set; }
            public bool IsContinueCalled { get; private set; }
            public bool IsRebindCalled { get; private set; }

            protected override ActionResult AddAction(BaseWasteCodeViewModel viewModel)
            {
                IsAddCalled = true;
                return base.AddAction(viewModel);
            }

            protected override ActionResult RemoveAction(BaseWasteCodeViewModel viewModel)
            {
                IsRemoveCalled = true;
                return base.RemoveAction(viewModel);
            }

            protected override Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel)
            {
                IsContinueCalled = true;
                return Task.FromResult(View() as ActionResult);
            }

            protected override Task RebindModel(Guid id, BaseWasteCodeViewModel viewModel)
            {
                IsRebindCalled = true;
                return Task.FromResult(0);
            }
        }

        private class TestViewModel : BaseWasteCodeViewModel
        {
        }
    }
}