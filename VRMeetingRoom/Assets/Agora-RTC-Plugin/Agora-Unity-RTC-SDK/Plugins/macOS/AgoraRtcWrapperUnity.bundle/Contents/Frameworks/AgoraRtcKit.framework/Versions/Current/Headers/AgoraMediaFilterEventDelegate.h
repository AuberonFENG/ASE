//
//  AgoraMediaFilterEventDelegate.h
//  Agora SDK
//
//  Created by LLF on 2020-9-21.
//  Copyright (c) 2020 Agora. All rights reserved.
//

#import <Foundation/Foundation.h>

/** 
 * The definition of extension context types.
 */
@interface AgoraExtensionContext : NSObject
/** 
 * Whether the uid is valid.
 * - YES: The uid is valid.
 * - NO: The uid is invalid.
 */
@property (assign, nonatomic) BOOL isValid;
/**
 * The ID of the user.
 * A uid of 0 indicates the local user, and a uid greater than 0 represents a remote user.
 */
@property (assign, nonatomic) NSUInteger uid;
/**
 * The provider name of the current extension.
 */
@property (copy, nonatomic) NSString * _Nullable providerName;
/** 
 * The extension name of the current extension.
 */
@property (copy, nonatomic) NSString * _Nullable extensionName;
@end

@protocol AgoraMediaFilterEventDelegate <NSObject>
@optional
/* Meida filter(audio filter or video filter) event callback
 */ 
- (void)onEventWithContext:(AgoraExtensionContext * __nonnull)context
            key:(NSString * __nullable)key
          value:(NSString * __nullable)value NS_SWIFT_NAME(onEventWithContext(_:key:value:));

- (void)onExtensionStartedWithContext:(AgoraExtensionContext * __nonnull)context NS_SWIFT_NAME(onExtensionStartedWithContext(_:));

- (void)onExtensionStoppedWithContext:(AgoraExtensionContext * __nonnull)context NS_SWIFT_NAME(onExtensionStoppedWithContext(_:));
 
- (void)onExtensionErrorWithContext:(AgoraExtensionContext * __nonnull)context
                   error:(int)error
                 message:(NSString * __nullable)message NS_SWIFT_NAME(onExtensionErrorWithContext(_:error:message:));
 
@end
