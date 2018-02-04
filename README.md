# WwiseUnityAudio
Very simple tool for Unity to convert a project with Wwise to Unity Audio in order to make WebGL builds.

**The objective of this tool isn't to reproduce Wwise** but to provide a way to have a project building for WebGL.
You made a game jam with Wwise and you are sad because you cannot put the web player on itch.io ?  **This tool is made for you.**

Here is some Wwise projects that also have a WebGL build thanks to this small tool : 
- Cook'Em Up : https://carbonara.itch.io/cookem-up

## Limitations :
- Only works for 2D projects for now
- Since WebGL doesn't support multithreading, audio filters like reverberation are not supported
- Not all Wwise features are implemented

## Installation :
- Clone this repository or download the asset package here : https://goo.gl/BCxPY7
- Copy the WebGLSupport folder or open the unity asset package in your project

## Instructions :
You can convert your Unity Wwise project into a Unity Audio only project with the following steps : 
- Replace all calls to Wwise (I.E AkSoundEngine.PostEvent()) by AudioEventManager calls (AudioEventManager.PostEvent())
```csharp
// Replace
public void OnGameBegin()
{
   AkSoundEngine.PostEvent("Music_Play", this.gameobject);
}

// With
public void OnGameBegin()
{
   AudioEventManager.PostEvent("Music_Play", this.gameobject);
}
```

- In you first scene, put in the hierarchy the AudioEventManager prefab and toogle 'Unity Audio'

<p align="center">
  <img src="https://github.com/Aredhele/WwiseUnityAudio/blob/master/WwiseUnityAudio/Assets/WebGLSupport/Images/ManagerExample.JPG" width="350"/>
</p>

- With the help of the event editor, try to reproduce all events and RTPCs

<p align="center">
  <img src="https://github.com/Aredhele/WwiseUnityAudio/blob/master/WwiseUnityAudio/Assets/WebGLSupport/Images/EditorExample.JPG" width="700"/>
</p>

- Finally, switch to WebGL platform and **move the Wwise folder out of the assets folder just for the build**
- Enjoy your WebGL build (and don't forget to put the Wwise folder back into your assets folder)
