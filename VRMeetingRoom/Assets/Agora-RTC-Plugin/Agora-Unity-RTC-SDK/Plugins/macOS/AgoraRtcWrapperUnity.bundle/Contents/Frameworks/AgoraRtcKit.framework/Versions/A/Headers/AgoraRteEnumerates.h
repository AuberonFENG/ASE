#import <Foundation/Foundation.h>


typedef NS_ENUM(NSUInteger, AgoraRteVideoRenderMode) {
  AgoraRteVideoRenderModeHidden = 0,
  AgoraRteVideoRenderModeFit = 1
};

typedef NS_ENUM(NSUInteger, AgoraRteVideoMirrorMode) {
  AgoraRteVideoMirrorModeAuto = 0,
  AgoraRteVideoMirrorModeEnabled = 1,
  AgoraRteVideoMirrorModeDisabled = 2
};

typedef NS_ENUM(NSUInteger, AgoraRtePlayerState) {
  AgoraRtePlayerStateIdle = 0,
  AgoraRtePlayerStateOpening = 1,
  AgoraRtePlayerStateOpenCompleted = 2,
  AgoraRtePlayerStatePlaying = 3,
  AgoraRtePlayerStatePaused = 4,
  AgoraRtePlayerStatePlaybackCompleted = 5,
  AgoraRtePlayerStateStopped = 6,
  AgoraRtePlayerStateFailed = 7
};

typedef NS_ENUM(NSUInteger, AgoraRtePlayerEvent) {
  AgoraRtePlayerEventSeekBegin = 0,
  AgoraRtePlayerEventSeekComplete = 1,
  AgoraRtePlayerEventSeekError = 2,
  AgoraRtePlayerEventBufferLow = 3,
  AgoraRtePlayerEventBufferRecover = 4,
  AgoraRtePlayerEventFreezeStart = 5,
  AgoraRtePlayerEventFreezeStop = 6,
  AgoraRtePlayerEventOneLoopPlaybackCompleted = 7,
  AgoraRtePlayerEventAuthenticationWillExpire = 8
};

typedef NS_ENUM(NSUInteger, AgoraRtePlayerMetadataType) {
  AgoraRtePlayerMetadataTypeSei = 0,
};

typedef NS_ENUM(NSUInteger, AgoraRteErrorCode) {
  AgoraRteOk = 0,
  AgoraRteErrorDefault = 1,
  AgoraRteErrorInvalidArgument = 2,
  AgoraRteErrorInvalidOperation = 3,
  AgoraRteErrorNetworkError = 4,
  AgoraRteErrorAuthenticationFailed = 5,
  AgoraRteErrorStreamNotFound = 6,
};