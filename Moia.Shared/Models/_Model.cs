﻿using Microsoft.AspNetCore.DataProtection;

namespace Moia.Shared.Models
{
    public interface _IModel
    {
        [Key]
        public int ID { get; set; }

        //[NotMapped]
        //public string EncId
        //{
        //    get
        //    {
        //        return this.ID.ToString().EncryptId();
        //    }
        //}
    }
    public class _Model
    {
       
        [Key]
        public int ID { get; set; }

        //[NotMapped]
        //public string EncId
        //{
        //    get
        //    {
        //        return this.ID.ToString().EncryptId();
        //    }
        //}

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

    }

    public static class IdEncryption
    {
        private static string EncryptionKey { get { return "sad24345rgfd234mfgDrt66wsswe466fE35"; } }

        private static IDataProtector Protector
        {
            get
            {
                IDataProtectionProvider dataProtectionProvider = DataProtectionProvider.Create("TafeelCoProtectorHashedv3");
                return dataProtectionProvider.CreateProtector(EncryptionKey);
            }
        }

        public static string EncryptId(this string text)
        {
            return Protector.Protect(text);
        }

        public static string DecryptId(this string text)
        {
            return Protector.Unprotect(text);
        }
    }
} 
