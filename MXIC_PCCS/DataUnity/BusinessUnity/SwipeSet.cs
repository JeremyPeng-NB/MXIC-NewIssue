﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using MXIC_PCCS.DataUnity.Interface;
using MXIC_PCCS.Models;
using Newtonsoft.Json;

namespace MXIC_PCCS.DataUnity.BusinessUnity
{
    public class SwipeSet : ISwipeSet, IDisposable
    {
        
            public PCCSContext _db = new PCCSContext();

            public MxicTestContext _dbMXIC = new MxicTestContext();

            //關閉資料庫
            #region IDisposable Support
            private bool disposedValue = false; // 偵測多餘的呼叫

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: 處置 Managed 狀態 (Managed 物件)。
                        _db.Dispose();
                    }

                    // TODO: 釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下方的完成項。
                    // TODO: 將大型欄位設為 null。

                    disposedValue = true;
                }
            }

            // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放 Unmanaged 資源的程式碼時，才覆寫完成項。
            // ~UserManagement() {
            //   // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            //   Dispose(false);
            // }

            // 加入這個程式碼的目的在正確實作可處置的模式。
            public void Dispose()
            {
                // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
                Dispose(true);
                // TODO: 如果上方的完成項已被覆寫，即取消下行的註解狀態。
                GC.SuppressFinalize(this);
            }
            #endregion

            public string UserList(string PoNo1, DateTime? Date)
            {

                var _List = _db.SwipeSets.Select(x => new { x.PoNo1, x.AttendType, x.SwipeStartTime, x.SwipeEndTime, x.EditID, x.Date });

                if (!string.IsNullOrWhiteSpace(PoNo1))
                {
                    _List = _List.Where(x => x.PoNo1.ToLower().Contains(PoNo1.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Date.ToString()))
                {
                    string s = Convert.ToDateTime(Date).Year.ToString() + "-" + Convert.ToDateTime(Date).Month.ToString("00");
                    //string s = "2021-6";
                    //string s = "2021-06";
                    _List = _List.Where(x => x.Date.ToString().Contains(s));
                    //TODO:搜尋功能失效，先測試另一行程式有無作用
                    //_List = _List.Where(x => x.Date.Day.ToString() );
                    //_List = _List.Where(x => x.Date == Date);
                    //_List = _List.Where(x => x.Date.ToString().Contains(x.Date.Year.ToString() + "-" + x.Date.Month.ToString()));
                }

                string Str = JsonConvert.SerializeObject(_List, Formatting.Indented);

                return (Str);
            }

            public string EditTimeDetail(string EditID)
            {
                string Str = "修改成功";

                var EditTimeDetail = _db.SwipeSets.Where(x => x.EditID.ToString() == EditID).Select(x => new { x.SwipeStartTime, x.SwipeEndTime });
                string responseStr = JsonConvert.SerializeObject(EditTimeDetail, Formatting.Indented);

                return (responseStr);
            }

            public string EditTime(string EditID, string SwipeStartTime, string SwipeEndTime)
            {
                string MessageStr = "修改成功";

                //Todo : 這些要先從當前修改的資料EditID，去反查出以下的資料

                var EditTimeStr = _db.SwipeSets.Where(x => x.EditID.ToString() == EditID).FirstOrDefault();

                //以下給值
                string PO = EditTimeStr.PoNo1;
                string AttendType = EditTimeStr.AttendType;
                DateTime StartTime = Convert.ToDateTime(EditTimeStr.Date);
                //DateTime EndTime = Convert.ToDateTime("2021-12-01");
                DateTime EndTime = Convert.ToDateTime(EditTimeStr.Date.Year.ToString() + "-12-31");
                //DateTime EndTime = DateTime.Parse(EditTimeStr.Date.Year.ToString()).AddDays(1).AddSeconds(-1);

                //以下會查出多筆 該PO 同班別 不同月份 的 資料
                var EditTime = _db.SwipeSets.Where(x => x.PoNo1 == PO & x.AttendType == AttendType & x.Date >= StartTime & x.Date <= EndTime);
                try
                {
                    foreach (var EditTimeItem in EditTime)
                    {
                        EditTimeItem.SwipeStartTime = SwipeStartTime;
                        EditTimeItem.SwipeEndTime = SwipeEndTime;
                    }

                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageStr = ex.ToString();
                }

                return (MessageStr);
            }
        
    }
}