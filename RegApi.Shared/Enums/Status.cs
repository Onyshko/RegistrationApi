using System.ComponentModel.DataAnnotations;

namespace RegApi.Shared.Enums
{
    public enum Status
    {
        [Display(Name = "In process")]
        InProcess,
        [Display(Name = "Finished")]
        Finished
    }
}
