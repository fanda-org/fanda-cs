﻿using System;
using System.Collections.Generic;

namespace Fanda.Service.Base
{
    public abstract class PagedListBase
    {
        public int? Page { get; set; }
        public int? PageCount { get; set; }
        public int? PageSize { get; set; }
        public int? ItemsCount { get; set; }
        public int? FirstRowOnPage => Math.Min((int)(((Page - 1) * PageSize) + 1), (int)LastRowOnPage);
        public int? LastRowOnPage => Math.Min((int)Page * (int)PageSize, (int)ItemsCount);
    }

    public class PagedList<T> : PagedListBase /*, IList<T>*/
        where T : class
    {
        public IList<T> Data { get; set; }
        #region Commented - IList implementation
        //private readonly IList<T> _list;
        //public PagedList()
        //{
        //    _list = new List<T>();
        //}

        //#region Implementation of IEnumerable

        //public IEnumerator<T> GetEnumerator()
        //{
        //    return _list.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}

        //#endregion

        //#region Implementation of ICollection<T>

        //public void Add(T item)
        //{
        //    _list.Add(item);
        //}

        //public void Clear()
        //{
        //    _list.Clear();
        //}

        //public bool Contains(T item)
        //{
        //    return _list.Contains(item);
        //}

        //public void CopyTo(T[] array, int arrayIndex)
        //{
        //    _list.CopyTo(array, arrayIndex);
        //}

        //public bool Remove(T item)
        //{
        //    return _list.Remove(item);
        //}

        //public int Count
        //{
        //    get { return _list.Count; }
        //}

        //public bool IsReadOnly
        //{
        //    get { return _list.IsReadOnly; }
        //}

        //#endregion

        //#region Implementation of IList<T>

        //public int IndexOf(T item)
        //{
        //    return _list.IndexOf(item);
        //}

        //public void Insert(int index, T item)
        //{
        //    _list.Insert(index, item);
        //}

        //public void RemoveAt(int index)
        //{
        //    _list.RemoveAt(index);
        //}

        //public T this[int index]
        //{
        //    get { return _list[index]; }
        //    set { _list[index] = value; }
        //}

        //#endregion

        //#region Additional Stuff

        //// Add new features 

        //#endregion
        #endregion
    }
}