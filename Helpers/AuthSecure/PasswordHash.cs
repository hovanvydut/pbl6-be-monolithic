namespace Monolithic.Helpers.Password;

public class PasswordHash
{
    public byte[] PasswordHashed { get; set; }

    public byte[] PasswordSalt { get; set; }

    public PasswordHash() { }

    public PasswordHash(byte[] PasswordHashed, byte[] PasswordSalt)
    {
        this.PasswordHashed = PasswordHashed;
        this.PasswordSalt = PasswordSalt;
    }
}