using System;
namespace XliffLib
{
    public class IdCounter
    {
        private int _fileId;
        private int _groupId;
        private int _unitId;

        public int GetNextFileId()
        {
            return ++_fileId;
        }

		public int GetNextGroupId()
		{
			return ++_groupId;
		}

		public int GetNextUnitId()
		{
            return ++_unitId;
		}
    }
}
