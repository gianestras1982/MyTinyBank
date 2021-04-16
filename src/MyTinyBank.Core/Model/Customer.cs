using MyTinyBank.Core.Constants;
using System;
using System.Collections.Generic;

namespace MyTinyBank.Core.Model
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string VatNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string CountryCode { get; set; }
        public CustomerType CustType { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public AuditInfo AuditInfo { get; set; }
        public string DateNowOnlyDate { get; set; }
        //An eixame kapoia imerominia tipou Date kai thelame
        //o xristis na tin simplirosei oposdipote tote tha eprepe
        //na tin kanoume nullable gia na tin piasoume apo mprosta.
        //Ta string den ta kanoume nullable giati ennountai null diladi
        //arxikopoiountai null.
        //Ta enus den ta kanoume null giati arxikopoiountai me to undefined.

        //Navigation properties
        public List<Account> Accounts { get; set; }

        public Customer()
        {
            CustomerId = Guid.NewGuid();
            IsActive = true;
            AuditInfo = new AuditInfo();
            DateNowOnlyDate = DateTime.Now.ToString("yyyy-MM-dd");
            CustType = CustomerType.PhysicalEntity;

            Accounts = new List<Account>();
        }
    }
}
