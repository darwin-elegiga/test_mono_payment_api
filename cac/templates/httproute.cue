package templates

#HTTPRoute: {
	apiVersion: "gateway.networking.k8s.io/v1"
	kind:       "HTTPRoute"
	metadata: {
		name:      string
		namespace: string
		labels?: {
			[string]: string
		}
	}
	spec: {
		parentRefs: [...{
			name:        string
			namespace?: string
			sectionName?: string
		}]
		hostnames?: [...string]
				rules?: [...{
						backendRefs: [...{
								name: string
								port: int
						}]
				}]
	}
}
