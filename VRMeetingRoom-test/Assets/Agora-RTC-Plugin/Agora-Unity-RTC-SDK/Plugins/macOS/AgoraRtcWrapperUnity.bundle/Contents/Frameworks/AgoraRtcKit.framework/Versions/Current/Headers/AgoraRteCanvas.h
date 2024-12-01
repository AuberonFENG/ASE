/**
 *
 * Agora Real Time Engagement
 * Copyright (c) 2024 Agora IO. All rights reserved.
 *
 */

#import <Foundation/Foundation.h>

#import "AgoraRteEnumerates.h"

#if TARGET_OS_IPHONE
#import <UIKit/UIKit.h>
typedef UIView AgoraRteView;
#elif TARGET_OS_MAC
#import <AppKit/AppKit.h>
typedef NSView AgoraRteView;
#endif

@class AgoraRteError;
@class AgoraRte;


__attribute__((visibility("default"))) @interface AgoraRteRect : NSObject
- (instancetype _Nonnull)init;

- (void)setX:(int32_t)x;
- (int32_t)x;

- (void)setY:(int32_t)y;
- (int32_t)y;

- (void)setWidth:(int32_t)width;
- (int32_t)width;

- (void)setHeight:(int32_t)height;
- (int32_t)height;
@end

__attribute__((visibility("default"))) @interface AgoraRteViewConfig : NSObject
- (instancetype _Nonnull)init;

- (void)setCropArea:(AgoraRteRect* _Nullable)cropArea error:(AgoraRteError* _Nullable)error;

- (AgoraRteRect* _Nullable)cropArea:(AgoraRteError* _Nullable)error;
@end


__attribute__((visibility("default"))) @interface AgoraRteCanvasInitialConfig : NSObject
- (instancetype _Nonnull)init;
@end


__attribute__((visibility("default"))) @interface AgoraRteCanvasConfig : NSObject
- (instancetype _Nonnull)init;

- (void)setVideoRenderMode:(AgoraRteVideoRenderMode)mode error:(AgoraRteError * _Nullable)error;

- (AgoraRteVideoRenderMode)videoRenderMode:(AgoraRteError * _Nullable)error;

- (void)setVideoMirrorMode:(AgoraRteVideoMirrorMode)mode error:(AgoraRteError* _Nullable)error;

- (AgoraRteVideoMirrorMode)videoMirrorMode:(AgoraRteError * _Nullable)error;

- (void)setCropArea:(AgoraRteRect* _Nonnull)cropArea error:(AgoraRteError * _Nullable)error;

- (AgoraRteRect* _Nonnull)cropArea:(AgoraRteError* _Nullable)error;

@end


__attribute__((visibility("default"))) @interface AgoraRteCanvas : NSObject

- (instancetype _Nonnull)initWithRte:(AgoraRte* _Nonnull)rte initialConfig:(AgoraRteCanvasInitialConfig * _Nullable)config error:(AgoraRteError * _Nullable)error;

- (void)getConfigs:(AgoraRteCanvasConfig* _Nonnull)config error:(AgoraRteError* _Nullable)error;

- (void)setConfigs:(AgoraRteCanvasConfig* _Nonnull)config cb:(void (^_Nullable)(AgoraRteCanvas* _Nonnull canvas, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

- (void)addView:(AgoraRteView * _Nonnull)view config:(AgoraRteViewConfig* _Nullable)config cb:(void (^_Nullable)(AgoraRteCanvas* _Nonnull canvas, AgoraRteView* _Nonnull view, id _Nullable cbData, AgoraRteError* _Nullable err))cb cbData:(id _Nullable)cbData;

@end
