//
//  MPInstanceProvider+Unity.h
//  MoPubSDK
//
//  Copyright (c) 2016 MoPub. All rights reserved.
//

#if __has_include(<MoPub/MoPub.h>)
    #import <MoPub/MoPub.h>
    #import <MoPub/MPInstanceProvider.h>
#else
    #import "MPInstanceProvider.h"
#endif

@class MPUnityRouter;

@interface MPInstanceProvider (Unity)

- (MPUnityRouter *)sharedMPUnityRouter;

@end
