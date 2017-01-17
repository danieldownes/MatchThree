Match-3 style game in C# by Daniel Downes

This project is mainly an exercise to demonstrate my personal coding style and ability.

The project also acts as a playground to try out different architectural and coding practises that can be easily shared (i.e. not under NDA) to those who are interested.


Todo:

Touch support
Swappable Gem Hint Visual (logic implemented)
Game Over UI


Coding Ideas: 

Vector2Int
Grid MVC
IMoveable, GemMoveable, BombMoveable - MVC
Object Factory, with Object Pool support
Unit Tests


Convenstions:

internal/public members start with Uppercase
Private fields start with underscore and lowercase to allow properties with the same name to start with a lowercase, and the backing field to be underscore lowercase




Notes:

'internal' instead of 'public'
I agree with the top answer here:
http://stackoverflow.com/questions/106941/should-i-use-internal-or-public-visibility-by-default

If a field should be private, but accessable from the editor, then don't use public so to ensure other classes don't access it, use:
	[SerializeField]
	private ...
As discussed here: http://answers.unity3d.com/questions/60461/warning-cs0649-field-is-never-assigned-to-and-will.html