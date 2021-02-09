using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels.Other;

namespace Web.Utilities.AppCode
{
    public static class DropdownTree
    {
        private const string _childsChar = "--";

        #region prop & ctor

        private static bool _showRoot;
        private static Int64 _rootID;
        private static Int64 _editedItemId;
        private static IList<BaseTreeviewViewModel> _dbTreeList;
        private static IList<BaseDropdownViewModel> _ddlList;
        private static string _ddlListHtml;
        private static string childSeperator;

        #endregion


        #region GetDdlList

        /// <summary>
        /// دریافت لیست جهت دراپ داون لیست معمولی
        /// </summary>
        public static IList<BaseDropdownViewModel> GetDdlList(IList<BaseTreeviewViewModel> vmTreeList, bool showRoot, int editedItemId)
        {
            _dbTreeList = new List<BaseTreeviewViewModel>();
            _ddlList = new List<BaseDropdownViewModel>();
            childSeperator = _ddlListHtml = string.Empty; 

            _showRoot = showRoot;
            _dbTreeList = vmTreeList;
            _editedItemId = editedItemId;

            var _rootItem = vmTreeList.OrderBy(c => c.Id).First();
            _rootID = _rootItem.Id;

            if (showRoot) {
                _ddlList.Add(new BaseDropdownViewModel
                {
                    Id = _rootItem.Id,
                    Title = string.Format("{0} {1}", GetChildSeperator(CalcLevel(_rootItem.Id)), _rootItem.Title)
                });
            }

            CreateDdlTreeList(_rootID);
            return _ddlList;
        }


        private static void CreateDdlTreeList(Int64 parentId)
        {
            foreach (BaseTreeviewViewModel item in _dbTreeList.Where(c => c.ParentId == parentId && c.Id != _editedItemId).ToList())
            {
                _ddlList.Add(new BaseDropdownViewModel
                {
                    Id = item.Id,
                    Title = string.Format("{0} {1}", GetChildSeperator(CalcLevel(item.Id)), item.Title)
                });

                if (item.HaveChild)
                {
                    if (parentId != _rootID)
                        childSeperator += _childsChar;

                    CreateDdlTreeList(item.Id);
                }
            }
        }

        #endregion


        #region GetGroupedDdlTreeHtml

        /// <summary>
        /// دریافت اچ تی ام ال دراپ داون لیست گروه بندی شده
        /// with <optgroup></optgroup> tag
        /// </summary>
        public static string GetGroupedDdlTreeHtml(IList<BaseTreeviewViewModel> vmTreeList, int editedItemId)
        {
            _dbTreeList = vmTreeList;
            _editedItemId = editedItemId;

            var _rootItem = vmTreeList.OrderBy(c => c.Id).First();
            _rootID = _rootItem.Id;

            CreateDdlTreeHtml(_rootID);
            return _ddlListHtml;
        }


        private static void CreateDdlTreeHtml(Int64 parentId)
        {
            string _endOptGroup = "";

            foreach (BaseTreeviewViewModel item in _dbTreeList.Where(c => c.ParentId == parentId).ToList())
            {

                if (item.HaveChild)
                {
                    _ddlListHtml += string.Format("<optgroup label='{0} {1}'>", GetChildSeperator(CalcLevel(item.Id)), item.Title);
                    _endOptGroup += "</optgroup>";
                }
                else
                {
                    if (CalcLevel(item.Id) == 1)
                    {
                         _ddlListHtml += string.Format("<optgroup label='{0}'>", item.Title);
                         _endOptGroup += "</optgroup>";
                    }


                    if (item.Id == _editedItemId)
                        _ddlListHtml += string.Format("<option selected='selected' value='{2}'>{0} {1}</option>", GetChildSeperator(CalcLevel(item.Id)), item.Title, item.Id);
                    else
                        _ddlListHtml += string.Format("<option value='{2}'>{0} {1}</option>", GetChildSeperator(CalcLevel(item.Id)), item.Title, item.Id);
                }

                CreateDdlTreeHtml(item.Id);
            }

            _ddlListHtml += _endOptGroup;
        }

        #endregion


        #region general methods

        private static int CalcLevel(Int64 id)
        {
            int level = 0;
            var currentItem = _dbTreeList.First(c => c.Id == id);

            while (currentItem.ParentId != null)
            {
                currentItem = _dbTreeList.FirstOrDefault(c => c.Id == currentItem.ParentId);
                if (currentItem == null)
                    break;    
                level++;

            }

            if (!_showRoot)
                return level - 1;

            return level;
        }

        private static string GetChildSeperator(int level)
        {
            string sp = "";

            for (int i = 0; i < level; i++)
                sp += _childsChar;

            return sp;
        }

        #endregion
    }
}