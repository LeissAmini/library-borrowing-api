using System.ComponentModel.DataAnnotations;
using Library.Api.Validators;

namespace Library.Api.DTOs
{
    public class ReturnRequest
    {
        [NonEmptyGuid]
        public Guid BorrowRecordId { get; set; }

        [NonEmptyGuid]
        public Guid MemberId { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}