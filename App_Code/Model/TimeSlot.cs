using System;

namespace LearnSite.Model
{
    public class TimeSlot
    {
        private int slotID;
        private string slotName;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private int displayOrder;

        public int SlotID
        {
            get { return slotID; }
            set { slotID = value; }
        }

        public string SlotName
        {
            get { return slotName; }
            set { slotName = value; }
        }

        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
    }
}