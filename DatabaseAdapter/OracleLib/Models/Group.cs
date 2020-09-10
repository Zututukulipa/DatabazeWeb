using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseAdapter.OracleLib.Models
{
    public class Group
    {

        public int GroupId { get; set; }


        public int TeacherId { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Maximal lenght is 10 characters")]
        public string Name { get; set; }

        public event EventHandler<int> CapacityChanged;
        private int _maxCapacity;
        public int MaxCapacity
        {
            get => _maxCapacity;
            set
            { 
                _maxCapacity = value;
                CapacityChanged?.Invoke(this, value);
        }
    }

        public int ActualCapacity { get; set; }

        public override string ToString()
        {
            return Name;
        }

       
    }
}
