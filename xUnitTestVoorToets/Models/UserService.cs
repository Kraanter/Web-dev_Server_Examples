namespace xUnitTestVoorToets;

public class UserService
{
	private IUserRepository _userRepository;
	
	public UserService(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}
	
	public User GetUser(string name)
	{
		return _userRepository.GetUser(name);
	}
}