using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MXIC_PCCS.DataUnity.Interface
{
    interface ISwipeSet
    {

        string UserList(string PoNo, DateTime? Date);

        string EditTimeDetail(string EditID);

        string EditTime(string EditID, string SwipeStartTime, string SwipeEndTime);

        string InsertSwipeSetTime(string PoNo, DateTime Date, string WorkShift, string SwipeStartTime, string SwipeEndTime);
        /// <summary>
        /// 刪除時間設定
        /// </summary>
        /// <param name="DeleteID"></param>
        /// <returns></returns>
        string DeleteTimeDetail(string DeleteID);
    }
}