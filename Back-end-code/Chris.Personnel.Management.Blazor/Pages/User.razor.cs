#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using Chris.Personnel.Management.Blazor.Services;
using Chris.Personnel.Management.Common.DropDownListItems;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.UICommand.DTO;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;
using Microsoft.AspNetCore.Components;

namespace Chris.Personnel.Management.Blazor.Pages
{
    public partial class User
    {
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IDropDownService DropDownService { get; set; }
        [Inject] public ToastService ToastService { get; set; }

        #region initial data for page
        public IEnumerable<UserFormViewModel> Users { get; set; } = new List<UserFormViewModel>();
        public IEnumerable<GenderDropDownListItem> GenderEnumItems { get; set; } = new List<GenderDropDownListItem>();
        public IEnumerable<IsEnabledDropDownListItem> IsEnabledEnumItems { get; set; } = new List<IsEnabledDropDownListItem>();
        public IEnumerable<RoleDropDownListViewModel> RoleEnumItems { get; set; } = new List<RoleDropDownListViewModel>();

        protected override async Task OnInitializedAsync()
        {
            Users = await UserService.GetUsersAsync();
            GenderEnumItems = await DropDownService.GetGenderEnumItemsAsync();
            IsEnabledEnumItems = await DropDownService.GetIsEnabledEnumItemsAsync();
            RoleEnumItems = await DropDownService.GetRoleEnumItemsAsync();

            //await BindItemQueryAsync(Users, new QueryPageOptions
            //{
            //    Filters = null,
            //    PageIndex = 1,
            //    PageItems = 10,
            //    Searchs = null,
            //    SearchText = null,
            //    SortName = null,
            //    SortOrder = SortOrder.Unset
            //});

            await base.OnInitializedAsync();
        }
        #endregion

        #region query data for page
        public UserFormViewModel SearchModel { get; set; } = new UserFormViewModel();

        protected IEnumerable<int> PageItemsSource => new int[] { 2, 4, 10, 20 };

        protected Task<QueryData<UserFormViewModel>> OnQueryAsync(QueryPageOptions options) => BindItemQueryAsync(Users, options);

        protected Task<QueryData<UserFormViewModel>> BindItemQueryAsync(IEnumerable<UserFormViewModel> items, QueryPageOptions options)
        {
            if (!string.IsNullOrEmpty(SearchModel.Name))
                items = items.Where(item =>
                    item.Name?.Contains(SearchModel.Name, StringComparison.OrdinalIgnoreCase) ?? false);
            if (!string.IsNullOrEmpty(options.SearchText))
                items = items.Where(item => (item.Name?.Contains(options.SearchText) ?? false)
                                            || (item.TrueName?.Contains(options.SearchText) ?? false));

            // 过滤
            var isFiltered = false;
            if (options.Filters.Any())
            {
                items = items.Where(options.Filters.GetFilterFunc<UserFormViewModel>());

                // 通知内部已经过滤数据了
                isFiltered = true;
            }

            // 排序
            //var isSorted = false;
            //if (!string.IsNullOrEmpty(options.SortName))
            //{
            //    // 外部未进行排序，内部自动进行排序处理
            //    var invoker = SortLambdaCache.GetOrAdd(typeof(UserFormViewModel), key => items.GetSortLambda().Compile());
            //    items = invoker(items, options.SortName, options.SortOrder);

            //    // 通知内部已经过滤数据了
            //    isSorted = true;
            //}

            // 设置记录总数
            var total = items.Count();

            // 内存分页
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            return Task.FromResult(new QueryData<UserFormViewModel>
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = isFiltered,
                IsSearch = !string.IsNullOrEmpty(SearchModel.Name) || !string.IsNullOrEmpty(SearchModel.TrueName)
            });
        }
        #endregion

        #region about add for page
        public Modal AddModal { get; set; }
        private Toast? Toast { get; set; }
        public UserAddUICommand UserAdd { get; set; } = new UserAddUICommand
        {
            User = new UserDTO()
        };

        public async Task AddModalSaveClick()
        {
            await UserService.AddUser(UserAdd).ContinueWith(task =>
            {
                AddModal.Toggle();
                Toast?.SetPlacement(Placement.TopEnd);
                ToastService.Show(new ToastOption
                {
                    Category = ToastCategory.Success,
                    Title = "新增成功",
                    Content = "用户数据新增成功，4 秒后自动关闭"
                });
            });
        }
        #endregion

        #region about edit for page
        public Modal EditModal { get; set; }
        #endregion
    }
}
