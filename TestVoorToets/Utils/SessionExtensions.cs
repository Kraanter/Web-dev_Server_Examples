using System.Text.Json;

namespace TestVoorToets.Utils;

/*
 * Deze klasse is een extensie van ISession.
 * Het is een klasse die je kan gebruiken om objecten in de sessie op te slaan.
 *
 * Gemaakt door: John Brouwers
 * Github: https://github.com/JohnBrouwers/CijferRegistratie/blob/master/CijferRegistratie/Tools/ExtensionMethods.cs
 */

public static class SessionExtensions
{
	public static T Get<T>(this ISession session, string key) where T: new()
	{
		var sessionData =  session.GetString(key);
		if (!string.IsNullOrEmpty(sessionData))
		{
			// Make sure to use the System.Text.Json namespace
			T? nullableT = JsonSerializer.Deserialize<T>(sessionData);
			if (nullableT != null)
			{
				return nullableT;
			}
		}
		return new T();   
	}
	public static void Set<T>(this ISession session, string key, T sessionData)
	{
		session.SetString(key, JsonSerializer.Serialize<T>(sessionData));
	}
}