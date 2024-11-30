/**
 *
 * Agora Real Time Engagement
 * Copyright (c) 2024 Agora IO. All rights reserved.
 *
 */

#import <Foundation/Foundation.h>

@class AgoraRteError;
@class AgoraRte;
@class AgoraRtePlayerCustomSourceProvider;
@class AgoraRteStream;
@class AgoraRteCanvas;
@class AgoraRtePlayerObserver;

__attribute__((visibility("default"))) @interface AgoraRtePlayerInitialConfig : NSObject

-(instancetype _Nonnull)init;

@end


__attribute__((visibility("default"))) @interface AgoraRtePlayerConfig : NSObject

-(instancetype _Nonnull)init;

- (void)setAutoPlay:(BOOL)autoPlay error:(AgoraRteError * _Nullable)error;
- (BOOL)autoPlay:(AgoraRteError * _Nullable)error;

- (void)setPlaybackSpeed:(int32_t)speed error:(AgoraRteError * _Nullable)error;
- (int32_t)playbackSpeed:(AgoraRteError * _Nullable)error;

- (void)setPlayoutAudioTrackIdx:(int)idx error:(AgoraRteError * _Nullable)error;
- (int)playoutAudioTrackIdx:(AgoraRteError * _Nullable)error;

- (void)setPublishAudioTrackIdx:(int)idx error:(AgoraRteError * _Nullable)error;
- (int)publishAudioTrackIdx:(AgoraRteError * _Nullable)error;

- (void)setAudioTrackIdx:(int)idx error:(AgoraRteError * _Nullable)error;
- (int)audioTrackIdx:(AgoraRteError * _Nullable)error;

- (void)setSubtitleTrackIdx:(int)idx error:(AgoraRteError * _Nullable)error;
- (int)subtitleTrackIdx:(AgoraRteError * _Nullable)error;

- (void)setExternalSubtitleTrackIdx:(int)idx error:(AgoraRteError * _Nullable)error;
- (int)externalSubtitleTrackIdx:(AgoraRteError * _Nullable)error;

- (void)setAudioPitch:(int32_t)pitch error:(AgoraRteError * _Nullable)error;
- (int32_t)audioPitch:(AgoraRteError * _Nullable)error;

- (void)setPlayoutVolume:(int32_t)volume error:(AgoraRteError * _Nullable)error;
- (int32_t)playoutVolume:(AgoraRteError * _Nullable)error;

- (void)setAudioPlaybackDelay:(int32_t)delay error:(AgoraRteError * _Nullable)error;
- (int32_t)audioPlaybackDelay:(AgoraRteError * _Nullable)error;

- (void)setAudioDualMonoMode:(int)mode error:(AgoraRteError * _Nullable)error;
- (int)audioDualMonoMode:(AgoraRteError * _Nullable)error;

- (void)setPublishVolume:(int32_t)volume error:(AgoraRteError * _Nullable)error;
- (int32_t)publishVolume:(AgoraRteError * _Nullable)error;

- (void)setLoopCount:(int32_t)count error:(AgoraRteError * _Nullable)error;
- (int32_t)loopCount:(AgoraRteError * _Nullable)error;

- (void)setJsonParameter:(NSString * _Nonnull)jsonParameter error:(AgoraRteError * _Nullable)error;
- (NSString * _Nullable)jsonParameter:(AgoraRteError * _Nullable)error;

@end


__attribute__((visibility("default"))) @interface AgoraRtePlayerStats : NSObject

- (instancetype _Nonnull)init;

@end

__attribute__((visibility("default"))) @interface AgoraRtePlayerInfo : NSObject
- (instancetype _Nonnull)init;
@end


__attribute__((visibility("default"))) @interface AgoraRtePlayer : NSObject

- (instancetype _Nonnull)initWithRte:(AgoraRte * _Nonnull)rte initialConfig:(AgoraRtePlayerInitialConfig * _Nullable)config error:(AgoraRteError * _Nullable)error;

- (void)preloadWithUrl:(NSString * _Nonnull)url error:(AgoraRteError * _Nullable)error;

- (void)openWithUrl:(NSString * _Nonnull)url startTime:(uint64_t)startTime cb:(void (^_Nullable)(AgoraRtePlayer * _Nonnull player, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

- (void)openWithCustomSourceProvider:(AgoraRtePlayerCustomSourceProvider * _Nonnull)provider startTime:(uint64_t)startTime cb:(void (^_Nullable)(AgoraRtePlayer* _Nonnull player, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

- (void)openWithStream:(AgoraRteStream * _Nonnull)stream cb:(void (^_Nullable)(AgoraRtePlayer* _Nonnull player, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

- (void)getStats:(void (^_Nonnull)(AgoraRtePlayer* _Nonnull player, AgoraRtePlayerStats* _Nonnull stats, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

- (void)setCanvas:(AgoraRteCanvas *_Nonnull)canvas error:(AgoraRteError * _Nullable)error;

- (void)play:(AgoraRteError * _Nullable)error;

- (void)stop:(AgoraRteError * _Nullable)error;

- (void)pause:(AgoraRteError * _Nullable)error;

- (void)seek:(uint64_t)newTime error:(AgoraRteError * _Nullable)error;

- (void)muteAudio:(BOOL)mute error:(AgoraRteError * _Nullable)error;

- (void)muteVideo:(BOOL)mute error:(AgoraRteError * _Nullable)error;

- (uint64_t)getPosition:(AgoraRteError * _Nullable)error;

- (void)getInfo:(AgoraRtePlayerInfo * _Nonnull)info error:(AgoraRteError * _Nullable)error;

- (void)getConfigs:(AgoraRtePlayerConfig * _Nonnull)config error:(AgoraRteError * _Nullable)error;

- (void)setConfigs:(AgoraRtePlayerConfig * _Nonnull)config cb:(void (^_Nullable)(AgoraRtePlayer* _Nonnull player, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

- (BOOL)registerObserver:(AgoraRtePlayerObserver *_Nonnull)observer error:(AgoraRteError * _Nullable)error;

- (BOOL)unregisterObserver:(AgoraRtePlayerObserver * _Nullable)observer error:(AgoraRteError * _Nullable)error;

@end
