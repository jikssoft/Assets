//
//  PubNativeNativeAdAdapter.m
//  AdSample
//
//  Created by 최치웅 on 2017. 7. 21..
//  Copyright © 2017년 LEE HEEDAE. All rights reserved.
//

#import "PubNativeNativeAdAdapter.h"
#import "MPCoreInstanceProvider.h"
#import "MPNativeAdConstants.h"

@interface PubNativeNativeAdAdapter()

@end

static const NSTimeInterval kMoPubRequiredSecondsForImpression = 1.0;
static const CGFloat kMoPubRequiredViewVisibilityPercentage = 0.5;

@interface PubNativeNativeAdAdapter() <MPAdDestinationDisplayAgentDelegate, MPStaticNativeAdImpressionTimerDelegate>

@property (nonatomic, strong) MPStaticNativeAdImpressionTimer *impressionTimer;
@property (nonatomic, readonly) MPAdDestinationDisplayAgent *destinationDisplayAgent;

@end

@implementation PubNativeNativeAdAdapter

@synthesize properties = _properties;
@synthesize defaultActionURL = _defaultActionURL;

- (instancetype)initWithNativeAd:(NSDictionary *)nativeAd {
    if (self = [super init]) {
        NSMutableDictionary *properties = [NSMutableDictionary dictionary];
        
        NSMutableArray * impressionTrackers = [[NSMutableArray alloc] init];
        NSMutableArray * clickTrackers = [[NSMutableArray alloc] init];
        
        for(NSDictionary * beacon in nativeAd[@"beacons"]) {
            if([beacon[@"type"] isEqualToString:@"impression"]) {
                if(beacon[@"data"][@"url"] != nil) {
                    [impressionTrackers addObject:beacon[@"data"][@"url"]];
                }
            } else if([beacon[@"type"] isEqualToString:@"click"]) {
                if(beacon[@"data"][@"url"] != nil) {
                    [clickTrackers addObject:beacon[@"data"][@"url"]];
                }
            }
        }
        
        _impressionTrackerURLs = impressionTrackers;
        _clickTrackerURLs = clickTrackers;
        _defaultActionURL = [NSURL URLWithString:nativeAd[@"link"]];
        
        for(NSDictionary * asset in nativeAd[@"assets"]) {
            NSLog(@"asset: %@", asset);
            if([asset[@"type"] isEqualToString:@"icon"]) {
                [properties setObject:asset[@"data"][@"url"] forKey:kAdIconImageKey];
            }
            if([asset[@"type"] isEqualToString:@"banner"]) {
                [properties setObject:asset[@"data"][@"url"] forKey:kAdMainImageKey];
            }
            if([asset[@"type"] isEqualToString:@"title"]) {
                [properties setObject:asset[@"data"][@"text"] forKey:kAdTitleKey];
            }
            if([asset[@"type"] isEqualToString:@"description"]) {
                [properties setObject:asset[@"data"][@"text"] forKey:kAdTextKey];
            }
            if([asset[@"type"] isEqualToString:@"cta"]) {
                [properties setObject:asset[@"data"][@"text"] forKey:kAdCTATextKey];
            }
        }
        
        _properties = properties;
        
        _destinationDisplayAgent = [[MPCoreInstanceProvider sharedProvider] buildMPAdDestinationDisplayAgentWithDelegate:self];
        
        self.impressionTimer = [[MPStaticNativeAdImpressionTimer alloc] initWithRequiredSecondsForImpression:kMoPubRequiredSecondsForImpression requiredViewVisibilityPercentage:kMoPubRequiredViewVisibilityPercentage];
        self.impressionTimer.delegate = self;
    }
    
    return self;
}

- (void)displayContentForURL:(NSURL *)URL rootViewController:(UIViewController *)controller {
    if (!controller) {
        return;
    }
    
    if (!URL || ![URL isKindOfClass:[NSURL class]] || ![URL.absoluteString length]) {
        return;
    }
    
    [self.destinationDisplayAgent displayDestinationForURL:URL];
}


#pragma mark - Impression tracking
- (void)willAttachToView:(UIView *)view {
    [self.impressionTimer startTrackingView:view];
}

- (void)trackImpression {
    [self.delegate nativeAdWillLogImpression:self];
}

#pragma mark - <MPAdDestinationDisplayAgentDelegate>
- (UIViewController *)viewControllerForPresentingModalView {
    return [self.delegate viewControllerForPresentingModalView];
}

- (void)displayAgentWillPresentModal {
    [self.delegate nativeAdWillPresentModalForAdapter:self];
}

- (void)displayAgentWillLeaveApplication {
    [self.delegate nativeAdWillLeaveApplicationFromAdapter:self];
}

- (void)displayAgentDidDismissModal {
    [self.delegate nativeAdDidDismissModalForAdapter:self];
}

@end
