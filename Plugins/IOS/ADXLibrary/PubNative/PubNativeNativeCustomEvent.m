//
//  PubNativeNativeCustomEvent.m
//  AdSample
//
//  Created by 최치웅 on 2017. 7. 21..
//  Copyright © 2017년 LEE HEEDAE. All rights reserved.
//

#import <AdSupport/ASIdentifierManager.h>

#import "PubNativeNativeCustomEvent.h"
#import "PubNativeNativeAdAdapter.h"

#import "MPInstanceProvider.h"
#import "MPLogging.h"

#import "MPNativeAd.h"
#import "MPNativeAdError.h"
#import "MPIdentityProvider.h"

@implementation PubNativeNativeCustomEvent

// app_token, zone_id
- (void)requestAdWithCustomEventInfo:(NSDictionary *)info {
    NSString *appToken = [info objectForKey:@"app_token"];
    NSString *zoneId = [info objectForKey:@"zone_id"];
    
    NSString *errorMsg = nil;
    if (!appToken) errorMsg = @"Invalid PubNative appToken";
    if (!zoneId) errorMsg = @"Invalid PubNative zoneId";
    
    if (errorMsg) {
        [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:MPNativeAdNSErrorForInvalidAdServerResponse(errorMsg)];
        return;
    }
    
    UIDevice *device = [UIDevice currentDevice];

    NSString * url = [NSString stringWithFormat:@"https://api.pubnative.net/api/v3/native?apptoken=%@&zoneid=%@&al=m&mf=revenuemodel&dnt=%d&os=ios&osver=%@&devicemodel=%@", appToken, zoneId, [MPIdentityProvider advertisingTrackingEnabled] ? 0 : 1, [device systemVersion], [device model]];
    
    if([MPIdentityProvider advertisingTrackingEnabled]) {
        url = [NSString stringWithFormat:@"%@&idfa=%@", url, [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString]];
    }
    
    NSURL * requestURL = [NSURL URLWithString:url];

    [[[NSURLSession sharedSession] dataTaskWithURL:requestURL completionHandler:^(NSData * _Nullable data, NSURLResponse * _Nullable response, NSError * _Nullable error) {

        if(error || ![response isKindOfClass:[NSHTTPURLResponse class]]) {
            return [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:MPNativeAdNSErrorForNetworkConnectionError()];
        }
        
        NSError *jsonError;
        NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData:data options:0 error:&jsonError];
        
        if (jsonError) {
            return [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:MPNativeAdNSErrorForNetworkConnectionError()];
        } else {
            NSLog(@"%@",jsonResponse[@"ads"]);
            if([jsonResponse[@"ads"] count] > 0) {
                NSDictionary * ad = jsonResponse[@"ads"][0];
                
                PubNativeNativeAdAdapter *adAdapter = [[PubNativeNativeAdAdapter alloc] initWithNativeAd:ad];
                MPNativeAd *interfaceAd = [[MPNativeAd alloc] initWithAdAdapter:adAdapter];
                
                [self.delegate nativeCustomEvent:self didLoadAd:interfaceAd];
            } else {
                return [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:MPNativeAdNSErrorForNoInventory()];
            }
        }
    }] resume];
}

@end
