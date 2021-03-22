using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BookLoan.Authorization
{
    public static class BookLoanOperations
    {
        public static OperationAuthorizationRequirement Create =   
          new OperationAuthorizationRequirement {Name=Constants.CreateOperationName};
        public static OperationAuthorizationRequirement Read = 
          new OperationAuthorizationRequirement {Name=Constants.ReadOperationName};  
        public static OperationAuthorizationRequirement Update = 
          new OperationAuthorizationRequirement {Name=Constants.UpdateOperationName}; 
        public static OperationAuthorizationRequirement Delete = 
          new OperationAuthorizationRequirement {Name=Constants.DeleteOperationName};
        public static OperationAuthorizationRequirement Approve = 
          new OperationAuthorizationRequirement {Name=Constants.ApproveOperationName};
        public static OperationAuthorizationRequirement Reject = 
          new OperationAuthorizationRequirement {Name=Constants.RejectOperationName};
        public static OperationAuthorizationRequirement Loan =
          new OperationAuthorizationRequirement { Name = Constants.LoanOperationName };
        public static OperationAuthorizationRequirement Return =
          new OperationAuthorizationRequirement { Name = Constants.ReturnOperationName };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string ApproveOperationName = "Approve";
        public static readonly string RejectOperationName = "Reject";
        public static readonly string LoanOperationName = "Loan";
        public static readonly string ReturnOperationName = "Return";

        public static readonly string BookLoanAdministratorsRole = "BookLoanAdministrators";
        public static readonly string BookLoanManagersRole = "BookLoanManagers";
    }
}