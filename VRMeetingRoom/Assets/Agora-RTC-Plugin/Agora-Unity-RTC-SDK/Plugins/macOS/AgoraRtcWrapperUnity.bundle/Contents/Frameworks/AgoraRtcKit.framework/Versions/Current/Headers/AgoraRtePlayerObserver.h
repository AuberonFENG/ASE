/**
 *
 * Agora Real Time Engagement
 * Copyright (c) 2024 Agora IO. All rights reserved.
 *
 */


#import <Foundation/Foundation.h>
#import "AgoraRtePlayer.h"
#import "AgoraRteEnumerates.h"

__attribute__((visibility("default"))) @interface AgoraRtePlayerObserver : NSObject

- (instancetype _Nonnull)init;

- (void)onStateChanged:(AgoraRtePlayerState)oldState newState:(AgoraRtePlayerState)newState error:(AgoraRteError * _Nullable)error;

- (void)onPositionChanged:(uint64_t)currentTime utcTime:(uint64_t)utcTime;

- (void)onResolutionChanged:(int)width height:(int)height;

- (void)onEvent:(AgoraRtePlayerEvent)event;

- (void)onMetadata:(AgoraRtePlayerMetadataType)type data:(NSData * _Nonnull)data;

- (void)onPlayerInfoUpdated:(AgoraRtePlayerInfo * _Nonnull)info;

- (void)onAudioVolumeIndication:(int32_t)volume;

@end
