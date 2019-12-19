using Business.Identity.Models;

namespace Business.Repository.Avatar
{
    /// <summary>
    /// Provides access to Kentico avatars.
    /// </summary>
    public interface IAvatarRepository : IRepository
    {
        /// <summary>
        /// Gets a computed unique file name and a binary of a user's avatar.
        /// </summary>
        /// <param name="user">The user to find an avatar for.</param>
        /// <returns>A named tuple with a filename and binary.</returns>
        (string fileName, byte[] binary) GetUserAvatar(MedioClinicUser user);

        /// <summary>
        /// Saves a new avatar binary to Kentico.
        /// </summary>
        /// <param name="user">User to save the avatar for.</param>
        /// <param name="avatarBinary">The avatar binary.</param>
        void UploadUserAvatar(MedioClinicUser user, byte[] avatarBinary);

        /// <summary>
        /// Creates a new user avatar.
        /// </summary>
        /// <param name="filePath">Physical path of the new avatar image.</param>
        /// <param name="avatarName">Avatar name.</param>
        /// <returns></returns>
        int CreateUserAvatar(string filePath, string avatarName);
    }
}