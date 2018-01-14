//
//  MobvistaAdapterHelper.m
//  MoPubSampleApp
//
//  Created by CharkZhang on 2017/6/8.
//  Copyright © 2017年 MoPub. All rights reserved.
//

#import "MobvistaAdapterHelper.h"

static BOOL mobvistaSDKInitialized = NO;

NSString *const kMobVistaErrorDomain = @"com.mobvista.iossdk";


@implementation MobvistaAdapterHelper

+(BOOL)isSDKInitialized{

    return mobvistaSDKInitialized;
}

+(void)sdkInitialized{

#ifdef DEBUG

    if (DEBUG) {
        NSLog(@"The version of current Mobvista Adapter is: %@",MobvistaAdapterVersion);
    }
#endif

    mobvistaSDKInitialized = YES;
}

@end
