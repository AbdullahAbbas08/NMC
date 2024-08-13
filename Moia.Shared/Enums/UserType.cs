namespace Moia.DoL.Enums
{
    public enum UserType
    {
        DataEntry=1,
        CommitteeManager,
        DepartmentManager,
        BranchManager,
        SuperAdmin,
        NegoiatedDepartmentManager,
        NegoiatedBranchManager,
        BranchDataEntry,
        None
    }

    public static class RoleCodes
    {
        public const string DataEntry = "DataEntry";
        public const string CommitteeManager = "CommitteeManager";
        public const string DepartmentManager = "DepartmentManager";
        public const string BranchManager = "BranchManager";
        public const string SuperAdmin = "SuperAdmin";
        public const string NegoiatedDepartmentManager = "NegoiatedDepartmentManager";
        public const string NegoiatedBranchManager = "NegoiatedBranchManager";
        public const string BranchDataEntry = "BranchDataEntry";
        public const string None = "None";
    }



    public static class UserTypeAr
    {
        public const string DataEntry = "مدخل بيانات";
        public const string CommitteeManager = "مدير الجمعية";
        public const string DepartmentManager = "مدير إدارة الدعوة والإرشاد";
        public const string BranchManager = "مدير الفرع";
        public const string SuperAdmin = "مدير عام";
        public const string NegoiatedDepartmentManager = "مفوض مدير إدارة الدعوة والإرشاد";
        public const string NegoiatedBranchManager = "مفوض مدير الفرع ";
        public const string BranchDataEntry = "مدخل بيانات الفرع";
        public const string None = "لا شيء";
    }



}
