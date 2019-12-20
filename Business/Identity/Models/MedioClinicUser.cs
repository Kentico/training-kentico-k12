using System;

using CMS.Membership;

namespace Business.Identity.Models
{
    /// <summary>
    /// A derived <see cref="Kentico.Membership.User"/> class created for the purpose of the Medio Clinic website.
    /// </summary>
    /// <remarks>Designed to contain all role-specific properties of all users.</remarks>
    public class MedioClinicUser : Kentico.Membership.User
    {
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Phone { get; set; }

        public string Nationality { get; set; }

        public int AvatarId { get; set; }

        /// <summary>
        /// Explicit default constructor due to the existence of <see cref="MedioClinicUser(UserInfo)"/>.
        /// </summary>
        public MedioClinicUser()
        {
        }

        /// <summary>
        /// Creates a <see cref="MedioClinicUser"/> object out of a <see cref="UserInfo"/> one.
        /// </summary>
        /// <param name="userInfo">The input object.</param>

        // Hotfix-independent variant (begin)
        /*
        public MedioClinicUser(UserInfo userInfo) : base(userInfo)
        */
        // Hotfix-independent variant (end)

        // HF 12.0.34+ variant (begin)
        public MedioClinicUser(UserInfo userInfo)
        // HF 12.0.34+ variant (end)
        {
            if (userInfo == null)
            {
                return;
            }

            // Hotfix-independent variant (begin)
            /* 
            DateOfBirth = source.GetDateTimeValue("UserDateOfBirth", DateTime.MinValue);
            Gender = (Gender)source.UserSettings.UserGender;
            City = source.GetStringValue("City", string.Empty);
            Street = source.GetStringValue("Street", string.Empty);
            Phone = source.UserSettings.UserPhone;
            Nationality = source.GetStringValue("Nationality", string.Empty);
            AvatarId = source.UserAvatarID;
            */
            // Hotfix-independent variant (end)

            // HF 12.0.34+ variant (begin)
            MapFromUserInfo(userInfo);
            // HF 12.0.34+ variant (end)
        }

        // HF 12.0.34+ variant (begin)
        public override void MapFromUserInfo(UserInfo source)
        {
            base.MapFromUserInfo(source);

            DateOfBirth = source.GetDateTimeValue("UserDateOfBirth", DateTime.MinValue);
            Gender = (Gender)source.UserSettings.UserGender;
            City = source.GetStringValue("City", string.Empty);
            Street = source.GetStringValue("Street", string.Empty);
            Phone = source.UserSettings.UserPhone;
            Nationality = source.GetStringValue("Nationality", string.Empty);
            AvatarId = source.UserAvatarID;
        }

        public override void MapToUserInfo(UserInfo target)
        {
            base.MapToUserInfo(target);

            target.UserAvatarID = AvatarId;
            target.UserSettings.UserDateOfBirth = DateOfBirth;
            target.UserSettings.UserGender = (int)Gender;
            target.UserSettings.UserPhone = Phone;
            target.SetValue("City", City);
            target.SetValue("Street", Street);
            target.SetValue("Nationality", Nationality);
        }
        // HF 12.0.34+ variant (end)
    }
}
