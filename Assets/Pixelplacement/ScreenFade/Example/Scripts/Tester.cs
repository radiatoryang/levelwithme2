/*---------------------------------------------------------------------------------
Tests out screen fade.

Author:	Bob Berkebile
Email:	bobb@pixelplacement.com
---------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {
	
	//-----------------------------------------------------------------------------
	// Public Variables
	//-----------------------------------------------------------------------------
	
	public AudioClip injurySound;
	public AudioClip cameraShutterSound;
	
	//-----------------------------------------------------------------------------
	// Private Variables
	//-----------------------------------------------------------------------------
	
	bool fadeDirection;
	Vector3 startingPosition;
	AudioSource cachedAudioSource;
	Transform cachedTransform;
	enum FadeType { Fade, Flash, Injury };
	FadeType currentFadeType;
	
	//-----------------------------------------------------------------------------
	// Init
	//-----------------------------------------------------------------------------
	
	void Awake(){
		cachedAudioSource = audio;
		cachedTransform = transform;
		startingPosition = cachedTransform.localPosition;
	}
	
	//-----------------------------------------------------------------------------
	// Event Registration
	//-----------------------------------------------------------------------------
	
	void OnEnable(){
		ScreenFade.OnFadeBegin += HandleFadeBegin;
		ScreenFade.OnFadeUpdate += HandleFadeUpdate;
		ScreenFade.OnFadeEnd += HandleFadeEnd;
	}
	
	void OnDisable(){
		ScreenFade.OnFadeBegin -= HandleFadeBegin;
		ScreenFade.OnFadeUpdate -= HandleFadeUpdate;
		ScreenFade.OnFadeEnd -= HandleFadeEnd;
	}
	
	//-----------------------------------------------------------------------------
	// Event Handlers
	//-----------------------------------------------------------------------------
	
	void HandleFadeBegin(){
		Debug.Log( "Fade Begin" );
		//reset position since the injury effect moves us around:
		cachedTransform.localPosition = startingPosition;
	}
	
	void HandleFadeUpdate( float percentage ){
		switch ( currentFadeType ) {
		
		case FadeType.Fade:
			Debug.Log( "Fade Updated: " + percentage );
			break;
			
		case FadeType.Flash:
			Debug.Log( "Flash Updated: " + percentage );
			break;
			
		case FadeType.Injury:
			Debug.Log( "Injury Updated: " + percentage );
			
			//generate random offsets that diminish in severity by the fade's percentage to shake the camera with:
			float randomXAmount = Random.Range( -percentage, percentage );
			float randomYAmount = Random.Range( -percentage, percentage );
			
			//create a vector to shake the camera with reduced a little so the effect isn't too crazy:
			Vector3 shakeAmount = new Vector3( randomXAmount, randomYAmount, 0 ) * .1f;
			
			//shake the camera by adding our shake amount to the camera's starting position:
			cachedTransform.localPosition = startingPosition + shakeAmount;
			break;
		}
	}
	
	void HandleFadeEnd(){
		Debug.Log( "Fade End" );
		//reset position since the injury effect moves us around:
		cachedTransform.localPosition = startingPosition;
	}
	
	//-----------------------------------------------------------------------------
	// OnGUI
	//-----------------------------------------------------------------------------
	
	void OnGUI(){
		//draw buttons:
		DrawFadeButton();
		DrawCameraFlashButton();
		DrawInjuryButton();
	}	
	
	//-----------------------------------------------------------------------------
	// Private Methods
	//-----------------------------------------------------------------------------
	
	void DrawFadeButton(){
		//decide text to display on the button based on state of fadeDirection:
		string fadeText = fadeDirection ? "down" : "up";
		
		if ( GUILayout.Button( "Fade " + fadeText ) ) {
			//set type of fadeDirection:
			currentFadeType = FadeType.Fade;
			
			//toggle state of fadeDirection:
			fadeDirection = !fadeDirection;
			
			//fade up or down based on state of fadeDirection:
			if (fadeDirection) {
				ScreenFade.Fade( Color.black, ScreenFade.CurrentAlpha, 1, 1, 0, false );
			}else{
				ScreenFade.Fade( Color.black, ScreenFade.CurrentAlpha, 0, 1, 0, false );
			}
		}		
	}
	
	void DrawCameraFlashButton(){
		if ( GUILayout.Button( "Flash/Lightning" ) ) {
			fadeDirection = false;
			currentFadeType = FadeType.Flash;
			ScreenFade.Fade( Color.white, 1, 0, .5f, 0, true );
			PlaySound( cameraShutterSound, 0 );
		}		
	}
	
	void DrawInjuryButton(){
		if ( GUILayout.Button( "Injury" ) ) {
			fadeDirection = false;
			currentFadeType = FadeType.Injury;
			ScreenFade.Fade( Color.red, .6f, 0, .35f, 0, true );
			cachedAudioSource.pitch = Random.Range ( .99f, 1.1f );
			PlaySound( injurySound, .1f );
		}
	}
	
	void PlaySound( AudioClip clip, float randomRange ){
		randomRange *= .5f;
		cachedAudioSource.pitch = Random.Range ( 1 -randomRange , 1 + randomRange );
		cachedAudioSource.PlayOneShot( clip );
	}
	
}
