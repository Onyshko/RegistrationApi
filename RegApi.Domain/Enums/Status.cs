using System.ComponentModel.DataAnnotations;

namespace RegApi.Domain.Enums
{
    public enum Status
    {
        [Display(Name = "In process")]
        InProcess,
        [Display(Name = "Finished")]
        Finished
    }
}
