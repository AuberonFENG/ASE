/**
 *
 * Agora Real Time Engagement
 * Copyright (c) 2024 Agora IO. All rights reserved.
 *
 */

#import <Foundation/Foundation.h>

#import "AgoraRteEnumerates.h"

__attribute__((visibility("default"))) @interface AgoraRteError : NSObject

- (instancetype _Nonnull)init;

- (void)setErrorWithCode:(AgoraRteErrorCode)code message:(NSString * _Nullable)message;

- (AgoraRteErrorCode)code;
- (NSString * _Nullable)message;

@end
