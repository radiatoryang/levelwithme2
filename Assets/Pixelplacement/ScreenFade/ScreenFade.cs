/*---------------------------------------------------------------------------------
Allows easy methods for fading the screen to and from a color using Unity's GUI 
system since most UI's are done with geometry this allows us a solution that will
(most of the time) be above everything else rendered on screen.

Author:	Bob Berkebile
Email:	bobb@pixelplacement.com
---------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;

[ AddComponentMenu ( "Pixelplacement/ScreenFade" ) ]
public class ScreenFade : MonoBehaviour {
	
	//-----------------------------------------------------------------------------
	// Events
	//-----------------------------------------------------------------------------
	
	public static event System.Action OnFadeBegin;
	public static event System.Action<float> OnFadeUpdate;
	public static event System.Action OnFadeEnd;
	
	//-----------------------------------------------------------------------------
	// Public Properties
	//-----------------------------------------------------------------------------
	
	public static Color CurrentColor{
		get{
			return currentColor;	
		}
	}	
	
	public static float CurrentAlpha{
		get{
			return currentColor.a;	
		}
	}
	
	public static bool IsFadingUp{
		get{
			return isFadingUp;	
		}
	}
	
	//-----------------------------------------------------------------------------
	// Private Variables
	//-----------------------------------------------------------------------------
	
	static Texture2D texture;
	static ScreenFade _instance;
	static Color baseColor = Color.black;
	static Color startColor;
	static Color currentColor;
	static Color endColor;
	static bool isFadingUp;
	
	//-----------------------------------------------------------------------------
	// Init
	//-----------------------------------------------------------------------------
	
	void Awake(){
		useGUILayout = false;	
	}
	
	//-----------------------------------------------------------------------------
	// Deallocation
	//-----------------------------------------------------------------------------
	
	void OnDestroy(){
		_instance = null;
	}
	
	//-----------------------------------------------------------------------------
	// GUI
	//-----------------------------------------------------------------------------
	
	void OnGUI(){
		if ( currentColor.a > 0 ) {
			GUI.color = currentColor;
			GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), texture );
		}
	}
	
	//-----------------------------------------------------------------------------
	// Public Methods
	//-----------------------------------------------------------------------------
	
	/// <summary>
	/// Changes the color with the option to retain the current alpha.
	/// </summary>
	/// <param name='color'>
	/// Color.
	/// </param>
	/// <param name='retainCurrentAlpha'>
	/// Retain current alpha.
	/// </param>
	public static void ChangeColor( Color color, bool retainCurrentAlpha ){
		CheckInstance();
		baseColor = color;
		if ( retainCurrentAlpha ) {
			baseColor.a = currentColor.a;
		}
		texture.SetPixel( 1, 1, baseColor );
		texture.Apply();
	}
	
	/// <summary>
	/// Fade with complete control over all features. To instantly jump to the startAlpha value before any delay begins, set jumpToStartAlpha to true - this is useful for a delayed scene fade in from black.
	/// </summary>
	/// <param name='color'>
	/// Color.
	/// </param>
	/// <param name='startAlpha'>
	/// Start alpha.
	/// </param>
	/// <param name='endAlpha'>
	/// End alpha.
	/// </param>
	/// <param name='duration'>
	/// Duration.
	/// </param>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='jumpToStartAlpha'>
	/// Jump to start alpha.
	/// </param>
	public static void Fade( Color color, float startAlpha, float endAlpha, float duration, float delay, bool jumpToStartAlpha ){
		
		CheckInstance();
		ChangeColor( color, false );
		
		startColor = baseColor;
		startColor.a = startAlpha;
		
		endColor = baseColor;
		endColor.a = endAlpha;
		
		if ( jumpToStartAlpha ) {
			currentColor.a = startAlpha;
		}
		
		_instance.StopAllCoroutines();
		_instance.StartCoroutine( _instance.DoFade( duration, delay ) );	
	}
	
	/// <summary>
	/// Fade the current color with complete control over all features. To instantly jump to the startAlpha value before any delay begins, set jumpToStartAlpha to true - this is useful for a delayed scene fade in from black.
	/// </summary>
	/// <param name='startAlpha'>
	/// Start alpha.
	/// </param>
	/// <param name='endAlpha'>
	/// End alpha.
	/// </param>
	/// <param name='duration'>
	/// Duration.
	/// </param>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public static void Fade( float startAlpha, float endAlpha, float duration, float delay, bool jumpToStartAlpha ){
		Fade( baseColor, startAlpha, endAlpha, duration, delay, jumpToStartAlpha );
	}
	
	/// <summary>
	/// Fade up the current color.
	/// </summary>
	/// <param name='duration'>
	/// Duration.
	/// </param>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public static void FadeUp( float duration, float delay ){
		Fade( baseColor, currentColor.a, 1, duration, delay, false );
	}
	
	/// <summary>
	/// Fade down the current color.
	/// </summary>
	/// <param name='duration'>
	/// Duration.
	/// </param>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public static void FadeDown( float duration, float delay ){
		Fade( baseColor, currentColor.a, 0, duration, delay, false );	
	}

	//-----------------------------------------------------------------------------
	// Private Methods
	//-----------------------------------------------------------------------------
	
	static void CheckInstance(){
		if ( _instance == null ) {
			
			//create singleton:
			GameObject screenFadeGameObject = new GameObject( "Screen Fade" );
			_instance = screenFadeGameObject.AddComponent<ScreenFade>();
			
			//create texture:
			texture = new Texture2D( 1, 1, TextureFormat.ARGB32, false );
			ChangeColor( currentColor, false );
		}
	}
	
	//-----------------------------------------------------------------------------
	// Coroutines
	//-----------------------------------------------------------------------------
	
	IEnumerator DoFade( float duration, float delay ){
		if ( startColor == endColor ) {
			yield break;
		}
		
		if ( delay > 0 ) {
			yield return new WaitForSeconds( delay );
		}
		
		if ( currentColor.a < endColor.a ) {
			isFadingUp = true;
		}else{
			isFadingUp = false;	
		}
		
		float startTime = Time.realtimeSinceStartup;
		
		if ( OnFadeBegin != null) { OnFadeBegin(); }
		
		while (true) {
			float percentage = ( Time.realtimeSinceStartup - startTime ) / duration;
			if ( OnFadeUpdate != null ) { OnFadeUpdate( percentage ); }
			currentColor = Color.Lerp( startColor, endColor, percentage );
			if ( percentage >= 1 ) {
				currentColor = endColor;
				if ( OnFadeEnd != null) { OnFadeEnd(); }
				yield break;
			}
			yield return null;
		}
	}
}
