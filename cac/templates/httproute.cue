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
			group?:       string
			kind?:        string
			name:        string
			namespace?: string
			sectionName?: string
		}]
		hostnames?: [...string]
		rules?: [...{
			backendRefs: [...{
				group?:  string
				kind?:   string
				name:    string
				port:    int
				weight?: int
			}]
			matches?: [...{
				path: {
					type:  string
					value: string
				}
			}]
		}]
	}
}
