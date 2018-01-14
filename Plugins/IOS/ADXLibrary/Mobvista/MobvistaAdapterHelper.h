//
//  MobvistaAdapterHelper.h
//  MoPubSampleApp
//
//  Created by CharkZhang on 2017/6/8.
//  Copyright © 2017年 MoPub. All rights reserved.
//

#import <Foundation/Foundation.h>

#define MobvistaAdapterVersion  @"1.3.0"

extern NSString *const kMobVistaErrorDomain;

@interface MobvistaAdapterHelper : NSObject


+(BOOL)isSDKInitialized;

+(void)sdkInitialized;

@end
