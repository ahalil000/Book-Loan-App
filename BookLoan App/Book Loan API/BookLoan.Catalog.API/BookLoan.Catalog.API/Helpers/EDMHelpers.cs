using BookLoan.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace BookLoan.Catalog.API.Helpers
{
    public class EdmHelpers
    { 
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<BookViewModel>("Books");
            return builder.GetEdmModel();
        }
    }
}
