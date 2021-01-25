using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXIC_PCCS.DataUnity.Interface
{
    interface ISwipeInfo
    {
        string CheckinList(string VendorName, string EmpID, string EmpName ,DateTime? StartTime, DateTime? EndTime, string AttendTypeSelect);

        string SwipeInfoDetail(string EditID);

        string EditSwipe(string EditID, string AttendTypeSelect, string Hour);

         string transform();
        void transform2(string StartTime, string EndTime,string PoNo);
    }
}
