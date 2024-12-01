/**
 *
 * Agora Real Time Engagement
 * Copyright (c) 2024 Agora IO. All rights reserved.
 *
 */

#import <Foundation/Foundation.h>

@class AgoraRteError;
@class AgoraRteObserver;

__attribute__((visibility("default"))) @interface AgoraRteInitialConfig : NSObject

- (instancetype _Nonnull)init;

@end

__attribute__((visibility("default"))) @interface AgoraRteConfig : NSObject

- (instancetype _Nonnull)init;

- (void)setAppId:(NSString * _Nullable)appId error:(AgoraRteError * _Nullable)error;
- (NSString* _Nullable)appId:( AgoraRteError * _Nullable)error;

- (void)setLogFolder:(NSString * _Nullable)logFolder error:(AgoraRteError * _Nullable)error;
- (NSString* _Nullable)logFolder:(AgoraRteError * _Nullable)error;

- (void)setLogFileSize:(size_t)logFileSize error:(AgoraRteError * _Nullable)error;
- (size_t)logFileSize:(AgoraRteError * _Nullable)error;

- (void)setAreaCode:(int32_t)areaCode error:(AgoraRteError * _Nullable)error;
- (int32_t)areaCode:(AgoraRteError * _Nullable)error;

- (void)setCloudProxy:(NSString * _Nullable)cloudProxy error:(AgoraRteError * _Nullable)error;
- (NSString * _Nullable)cloudProxy:(AgoraRteError * _Nullable)error;

- (void)setJsonParameter:(NSString * _Nullable)jsonParameter error:(AgoraRteError * _Nullable)error;
- (NSString * _Nullable)jsonParameter:(AgoraRteError * _Nullable)error;

@end

__attribute__((visibility("default"))) @interface AgoraRte : NSObject

+ (instancetype _Nonnull)getFromBridge:(AgoraRteError * _Nullable)error;

- (instancetype _Nonnull)initWithInitialConfig:(AgoraRteInitialConfig * _Nullable)config error:(AgoraRteError * _Nullable)error;

- (BOOL)initMediaEngine:(void (^ _Nullable)(AgoraRte* _Nonnull rte, id _Nullable cbData, AgoraRteError* _Nullable err) )cb cbData:(id _Nullable)cbData error:(AgoraRteError * _Nullable)error;

- (void)getConfigs:(AgoraRteConfig *_Nonnull)config error:(AgoraRteError * _Nullable)error;

- (void)setConfigs:(AgoraRteConfig * _Nonnull)config cb:(void (^ _Nullable)(AgoraRte * _Nonnull rte, id _Nonnull cbData, AgoraRteError* _Nonnull err) )cb cbData:(id _Nullable)cbData;

- (BOOL)registerObserver:(AgoraRteObserver * _Nonnull)observer error:(AgoraRteError * _Nullable)error;

- (BOOL)unregisterObserver:(AgoraRteObserver * _Nullable)observer error:(AgoraRteError * _Nullable)error;

@end
