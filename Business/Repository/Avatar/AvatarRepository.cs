using CMS.Membership;

using Business.Identity.Models;

namespace Business.Repository.Avatar
{
    public class AvatarRepository : IAvatarRepository
    {
        public (string fileName, byte[] binary) GetUserAvatar(MedioClinicUser user)
        {
            var avatarInfo = AvatarInfoProvider.GetAvatarInfo(user.AvatarId);

            if (avatarInfo != null)
            {
                return ($"{avatarInfo.AvatarGUID}{avatarInfo.AvatarFileExtension}", avatarInfo.AvatarBinary);
            }

            return (null, null);
        }
        
        public void UploadUserAvatar(MedioClinicUser user, byte[] avatarBinary)
        {
            var avatarInfo = AvatarInfoProvider.GetAvatarInfo(user.AvatarId);

            if (avatarInfo != null)
            {
                avatarInfo.AvatarBinary = avatarBinary;
                AvatarInfoProvider.SetAvatarInfo(avatarInfo);
            }
        }

        public int CreateUserAvatar(string filePath, string avatarName)
        {
            var newAvatar = new AvatarInfo(filePath);
            newAvatar.AvatarName = avatarName ?? string.Empty;
            newAvatar.AvatarType = AvatarInfoProvider.GetAvatarTypeString(AvatarTypeEnum.User);
            newAvatar.AvatarIsCustom = true;
            AvatarInfoProvider.SetAvatarInfo(newAvatar);

            return newAvatar.AvatarID;
        }
    }
}
