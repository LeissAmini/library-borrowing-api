using System.ComponentModel.DataAnnotations;
using Library.Api.Validators;

namespace Library.Api.DTOs
{
    public class BorrowRequest
    {
        [NonEmptyGuid]
        public Guid BookId { get; set; }

        [NonEmptyGuid]
        public Guid MemberId { get; set; }

        public DateTime BorrowDate { get; set; }
    }
}