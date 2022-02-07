using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Infrastructure.BaseTypes.Entities
{
    public abstract class BaseEntity<T> : IBaseEntity<T>
    {
        protected BaseEntity()
        {
            _createdDate = DateTime.Now;
            _modifiedDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        object IBaseEntity.Id => Id;

        private DateTime? _modifiedDate;
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate
        {
            get => _modifiedDate ?? DateTime.Now;
            set => _modifiedDate = value;
        }

        private DateTime? _createdDate;
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get => _createdDate ?? DateTime.Now;
            set => _createdDate = value;
        }

        public string ModifiedBy { get; set; }
    }

    public interface IBaseEntity
    {
        object Id { get; }
        DateTime CreatedDate { get; }
        DateTime ModifiedDate { get; }
    }

    public interface IBaseEntity<T> : IBaseEntity
    {
        new T Id { get; set; }
    }
}
